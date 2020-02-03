using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCommerceApp.Models
{
    public class Alerts
    {
        //the alerts class that are data will bind to, feel free to modify 
        public long AlertID { get; set; }
        public string Alert { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public long Zip { get; set; }
        public string ParameterOne { get; set; }
        public string ParameterTwo { get; set; }
    }
}