using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCommerceApp.Models
{
    //transaction class to bind data
    public class Transaction
    {
        public long TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string City { get; set; }
        public long Zip { get; set; }
        public int TransactionDateMonth { get; set; }
        public long TransactionDateDay { get; set;  }
        public long TransactionDateYear { get; set; }
        public string Date { get; set; }
        public void setDate()
        {
            Date = TransactionDateMonth.ToString()+"/"+TransactionDateDay.ToString()+"/20"+TransactionDateYear.ToString();
        }

    }
}