using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MyCommerceApp.ApplicationConfiguration
{

    public class AppConfiguration
    {
        public string DataConnection;

        public AppConfiguration()
        {
            DataConnection = ConfigurationManager.AppSettings["DataConnection"];
        }

    }
    
}