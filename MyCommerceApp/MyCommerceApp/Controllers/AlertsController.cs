using MyCommerceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCommerceApp.ApplicationConfiguration;
using System.Data;
using PagedList;

namespace MyCommerceApp.Controllers
{


    public class AlertsController : Controller
    {
        private AppConfiguration applicationConfig = new AppConfiguration();
        private AppSQLiteDB alertTypeData;
        private AppSQLiteDB custAlertTypeData;
        private AppSQLiteDB alertData;
        private string datapath;

        public AlertsController()
        {
            datapath = applicationConfig.DataConnection;
            alertTypeData = new AppSQLiteDB(datapath);
            custAlertTypeData = new AppSQLiteDB(datapath);
            alertData = new AppSQLiteDB(datapath);
        }
        public ActionResult Alerts(int? page)
        {           
            alertData.CreateConnection();
            alertData.SetAlertTable();

            ViewBag.alerts = alertData.AlertTable;

            List<Alerts> alerts = new List<Alerts>();
            foreach (var row in alertData.AlertTable.Rows)
            {
                var dataRow = (row as DataRow);
                var alert = new Alerts();

                alert.AlertID = dataRow.Field<long>("AlertID");
                alert.Alert = dataRow.Field<string>("Alert");
                alert.Description = dataRow.Field<string>("Description");
                alert.Amount = dataRow.Field<string>("Amount");
                alert.City = dataRow.Field<string>("City");
                alert.State = dataRow.Field<string>("State");
                alert.Zip = dataRow.Field<long>("Zip");
                alerts.Add(alert);
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(alerts.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult ManageAlerts()
        {            
            alertTypeData = new AppSQLiteDB(datapath);
            custAlertTypeData = new AppSQLiteDB(datapath);
            alertData = new AppSQLiteDB(datapath);
            ViewBag.AlertTypes = alertTypeData;
            ViewBag.AlertSettingsList = ProcessAlertSettings();

            return View(ProcessAlertSettings());
        }
        [HttpPost]
        public ActionResult ManageAlerts(FormCollection alertForm)
        {
            alertTypeData = new AppSQLiteDB(datapath);
            custAlertTypeData = new AppSQLiteDB(datapath);
            alertData = new AppSQLiteDB(datapath);
            ViewBag.AlertTypes = alertTypeData;
            UpdateAlertSettings(alertForm);
            var updatedSettings = ProcessAlertSettings();
            return View(updatedSettings);
        }

        private void UpdateAlertSettings(FormCollection alertForm)
        {
            foreach(var key in alertForm.AllKeys)
            {
                alertData.UpdateCustomerAlertSettings(key, alertForm[key]);
            }
        }

        private List<AlertSettings> ProcessAlertSettings()
        {
            alertTypeData.CreateConnection();

            alertTypeData.GetAlertTypes();
            alertTypeData.GetCustomerAlertSettings();

            var alertSettingsList = new List<AlertSettings>();


            for (int i = 0; i < alertTypeData.AlertTypeTable.Rows.Count; i++)
            {
                var typeRow = alertTypeData.AlertTypeTable.Rows[i];
                for (int j = 0; j < alertTypeData.CustomerAlertSettings.Rows.Count; j++)
                {
                    var alertSettings = new AlertSettings();
                    var row = alertTypeData.CustomerAlertSettings.Rows[j];
                    if (row[0].ToString() == typeRow[0].ToString())
                    {
                        alertSettings.AlertId = (long)row[0];
                        alertSettings.Setting = (string)row[2];
                        alertSettings.Active = (bool)row[3];
                        alertSettingsList.Add(alertSettings);

                    }
                }
            }

            var orderedAlertSettingsList = alertSettingsList.OrderBy(x => x.AlertId);

            int alertId = 1;

            while (alertId <= 4)
            {
                var alertSettings = new AlertSettings();
                var existingId = orderedAlertSettingsList.Any(x => x.AlertId == alertId);
                if (existingId == false)
                {
                    alertSettings.AlertId = alertId;
                    alertSettings.Active = false;
                    alertSettingsList.Add(alertSettings);
                }
                alertId++;
            }
            return alertSettingsList.OrderBy(x => x.AlertId).ToList();
        }
    }
}