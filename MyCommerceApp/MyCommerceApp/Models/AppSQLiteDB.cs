using System;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;

namespace MyCommerceApp.Models
{
    public class AppSQLiteDB: DbContext
    {
        private SQLiteDataAdapter DataAdapter { get; set; }
        private SQLiteConnection SQLiteConn { get; set; }
        public string DataSource { get; set; }
        public DataTable CustomTable { get; set; }
        public DataTable TransactionTable { get; set; }
        public DataTable AlertTable { get; set; }
        public DataTable CustomerTable { get; set; }
        public DataTable AccountTable { get; set; }
        public DataTable AlertTypeTable { get; set; }
        public DataTable CustomerAlertSettings { get; set; }
        
        public AppSQLiteDB(string dataSource)
        {
            CustomTable = new DataTable();
            DataSource = dataSource;
            SQLiteConn = CreateConnection();
        }

         public SQLiteConnection CreateConnection()
        {
            SQLiteConn = new SQLiteConnection(DataSource);
            try
            {
                SQLiteConn.Open();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return SQLiteConn;
        }

        public void CloseConn()
        {
            SQLiteConn.Close();
        }
        
        public void SetTransTable(string sqlCmd)
        {
            DataAdapter = new SQLiteDataAdapter();
            TransactionTable = new DataTable();
            DataAdapter.SelectCommand = new SQLiteCommand(sqlCmd, SQLiteConn);
            DataAdapter.Fill(TransactionTable);
        }

        public void SetAlertTable()
        {
            string sql = "SELECT * FROM [Alert]";
            DataAdapter = new SQLiteDataAdapter();
            AlertTable = new DataTable();
            DataAdapter.SelectCommand = new SQLiteCommand(sql, SQLiteConn);
            DataAdapter.Fill(AlertTable);
        }

        public void GetAlertTypes()
        {
            string sql = "SELECT AlertTypeID, AlertName FROM AlertTypes; ";
            DataAdapter = new SQLiteDataAdapter();
            AlertTypeTable = new DataTable();
            CustomerAlertSettings = new DataTable();
            DataAdapter.SelectCommand = new SQLiteCommand(sql, SQLiteConn);
            DataAdapter.Fill(AlertTypeTable);
        }
        public void GetCustomerAlertSettings()
        {
            string sql = "SELECT AlertID, CustomerID, Setting, Active FROM AlertSettings;";
            DataAdapter = new SQLiteDataAdapter();
            DataAdapter.SelectCommand = new SQLiteCommand(sql, SQLiteConn);
            DataAdapter.Fill(CustomerAlertSettings);
        }
        public void UpdateCustomerAlertSettings(string alertID, string data)
        {
            string active = "";
            string description = "";
            if (data.ToUpper().Contains("TRUE"))
            {
                active = "true";
                var alertInfo = data.Split(',');
                if (alertInfo.Length >= 3)
                {
                    description = alertInfo[0];
                }
                if (alertID == "2")
                {
                    description = alertInfo[0] + " " +alertInfo[1];
                }
            }
            else
                active = "false";
            
            string sql = $@"INSERT INTO AlertSettings(
                              AlertID,
                              CustomerID,
                              Setting,
                              Active
                          )
                          VALUES(
                              {alertID},
                              1,
                              '{description}',
                              {active}
                          ) ON CONFLICT(AlertID) DO update set Active = excluded.Active, CustomerID = excluded.CustomerID, Setting = excluded.Setting where AlertID = {alertID}"; 
            DataAdapter = new SQLiteDataAdapter();
            CustomerAlertSettings = new DataTable();
            DataAdapter.SelectCommand = new SQLiteCommand(sql, SQLiteConn);
            DataAdapter.Fill(CustomerAlertSettings);
        }
        public void SetAccountTable(string id, bool getName)
        {
            string sql = "";
            if (getName)
            {
                sql = "SELECT AccountID, Type, Balance, Fname FROM [Account] JOIN [Customer] WHERE " + id + " = AccountID";
            }
            else
                sql = "SELECT AccountID, Type, Balance FROM [Account] WHERE " + id + " = AccountID";
            DataAdapter = new SQLiteDataAdapter();
            AccountTable = new DataTable();
            DataAdapter.SelectCommand = new SQLiteCommand(sql, SQLiteConn);
            DataAdapter.Fill(AccountTable);
        }

        public void SetCustomerTable(string id)
        {
            string sql = "SELECT Fname, Lname, City, State, Zip FROM [Customer] WHERE " + id + " = AccountID";
            DataAdapter = new SQLiteDataAdapter();
            CustomerTable = new DataTable();
            DataAdapter.SelectCommand = new SQLiteCommand(sql, SQLiteConn);
            DataAdapter.Fill(CustomerTable);
        }
        //When implementing makeQuery use the custom_table attribute
        public void MakeQuery(string sqlCmd)
        {
            CustomTable = new DataTable();
            DataAdapter = new SQLiteDataAdapter(sqlCmd, SQLiteConn);
            DataAdapter.Fill(CustomTable);
        }
    };
}