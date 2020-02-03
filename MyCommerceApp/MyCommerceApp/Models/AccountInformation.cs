using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyCommerceApp.Models
{
    public class AccountInformation
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String AccountNumber { get; set; }
        public String AccountType { get; set; }
        public AccountInformation (String fName, String lName, String accountNum, String accountType)
        {
            FirstName = fName;
            LastName = lName;
            AccountNumber = accountNum;
            AccountType = accountType;

        }
    }
}