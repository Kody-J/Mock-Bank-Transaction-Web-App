using MyCommerceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using PagedList;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;
using MyCommerceApp.ApplicationConfiguration;




namespace MyCommerceApp.Controllers
{


public class AccountController : Controller
    {
        
        private AppConfiguration applicationConfig = new AppConfiguration();
        private string datapath;
        private AppSQLiteDB database;
        //test
        private decimal balance = 5000.00m;
        private decimal lastTransaAmnt = 0;
        private string accNum = "211111110";

        public AccountController()
        {
            datapath = applicationConfig.DataConnection;
            database = new AppSQLiteDB(datapath);
        }

        // GET: Account
        public ActionResult Summary()
        {
            return View();
        }

        //our "home page"
        public ActionResult Activity(string sortOrder, string currentFilter, string searchString, bool ? export, int? page, int? size)
        {
            datapath = applicationConfig.DataConnection;
            database = new AppSQLiteDB(datapath);
            database.CreateConnection();
            database.SetAccountTable(accNum, true);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstName = database.AccountTable.Rows[0].ItemArray[3] + "'s";
            ViewBag.AccType = database.AccountTable.Rows[0].ItemArray[1];
            ViewBag.AccNumber = database.AccountTable.Rows[0].ItemArray[0].ToString();
            ViewBag.AmountSortParm = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            string sql;
            switch (sortOrder)
            {
                case "date_asc":
                    sql = "SELECT TransactionID, Amount, Description, Type, TransactionDateMonth, TransactionDateDay, TransactionDateYear, City, State, Zip FROM [Transaction] WHERE " + accNum + " = AccountID " +
                "ORDER BY TransactionID ASC";
                    database.SetTransTable(sql);
                    int length = database.TransactionTable.Rows.Count;
                    lastTransaAmnt = database.TransactionTable.Rows[length-1].Field<decimal>("Amount");
                    break;
                case "Amount":
                    sql = "SELECT TransactionID, Amount, Description, Type, TransactionDateMonth, TransactionDateDay, TransactionDateYear, City, State, Zip FROM [Transaction] WHERE " + accNum + " = AccountID " +
               "ORDER BY Amount ASC";
                    database.SetTransTable(sql);
                    break;
                case "amount_desc":
                    sql = "SELECT TransactionID, Amount, Description, Type, TransactionDateMonth, TransactionDateDay, TransactionDateYear, City, State, Zip FROM [Transaction] WHERE " + accNum + " = AccountID " +
               "ORDER BY Amount DESC";
                    database.SetTransTable(sql);
                    break;
                default: //Date descending
                    sql = "SELECT TransactionID, Amount, Description, Type, TransactionDateMonth, TransactionDateDay, TransactionDateYear, City, State, Zip FROM [Transaction] WHERE " + accNum + " = AccountID " +
                        "ORDER BY TransactionID DESC";
                    database.SetTransTable(sql);
                    lastTransaAmnt = database.TransactionTable.Rows[0].Field<decimal>("Amount");
                    break;
            }

            List<Transaction> transactions = new List<Transaction>();
            foreach (var row in database.TransactionTable.Rows)
            {
                var dataRow = (row as DataRow);
                var transaction = new Transaction();

                transaction.TransactionID = dataRow.Field<long>("TransactionID");
                transaction.Amount = dataRow.Field<decimal>("Amount");
                transaction.City = dataRow.Field<string>("City");
                transaction.TransactionDateDay = dataRow.Field<long>("TransactionDateDay");
                transaction.TransactionDateMonth = dataRow.Field<int>("TransactionDateMonth");
                transaction.TransactionDateYear = dataRow.Field<long>("TransactionDateYear");
                transaction.setDate();
                transaction.Zip = dataRow.Field<long>("Zip");
                transaction.Description = dataRow.Field<string>("Description");
                transaction.Type = dataRow.Field<string>("Type");
                transactions.Add(transaction);
            }

            List<Transaction> transactionsSearch = new List<Transaction>();
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();

                foreach (var item in transactions)
                {
                    if (item.Amount.ToString().ToUpper().Contains(searchString) || item.Description.ToUpper().Contains(searchString)
                        || item.Type.ToUpper().Contains(searchString) || item.Date.ToUpper().Contains(searchString)
                        || item.Zip.ToString().ToUpper().Contains(searchString) || item.City.ToUpper().Contains(searchString))
                    {
                        transactionsSearch.Add(item);
                    }
                }
            }

            ViewBag.exportYes = export == false ?  true : false;
            if (ViewBag.exportYes)
            {
                GenerateExcel(database.TransactionTable, ViewBag.FirstName, ViewBag.Type);
            }

           
            ViewBag.Balance = balance;
            ViewBag.LastTransAmnt = lastTransaAmnt;
           
            int pageSize;
            if (size == 0)
            {
                pageSize = transactions.Count();
            }
            else
                pageSize = (size ?? 15);
            ViewBag.pgSize = pageSize;
            int pageNumber = (page ?? 1);
            if (transactionsSearch.Count() > 0)
                return View(transactionsSearch.ToPagedList(pageNumber, pageSize));
            return View(transactions.ToPagedList(pageNumber, pageSize));
        }

    
        protected void GenerateExcel(DataTable trans, string name, string type)
        {
            string fileName = "TransactionReport";
            string currentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string finalFileNameWithPath = string.Empty;

            fileName = string.Format("{0}_{1}", fileName, DateTime.Now.ToString("dd-MM-yyy"));
            finalFileNameWithPath = string.Format("{0}\\{1}.xlsx", currentDirectoryPath, fileName);

            
            //Delete exxisting file with same file name
            if (System.IO.File.Exists(finalFileNameWithPath))
                System.IO.File.Delete(finalFileNameWithPath);

            var newFile = new FileInfo(finalFileNameWithPath);

            using (var package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Transaction Report");
                worksheet.Cells["A1"].LoadFromDataTable(trans, true, OfficeOpenXml.Table.TableStyles.None);
                package.Workbook.Properties.Title = "Transaction Report 1";
                package.Workbook.Properties.Author = "Commerce Bank";
                package.Workbook.Properties.Subject = "Transaction Activity";

                package.Save();

                MessageBox.Show(string.Format("File name '{0}' saved in current users '\\Downloads' Directory.", fileName)
                    , "File generated successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
    }
}

