using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCommerceApp.Models
{
    public class AlertSettings
    {
        public bool Active { get; set; }
        public long AlertId { get; set; }
        public string Setting { get; set; }
    }
}