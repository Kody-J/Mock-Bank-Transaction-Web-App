# Mock-Bank-Transaction-Web-App
Using ASP.Net MVC with Entity and Bootstrap frameworks and a SQLite DB


This is an experiment with creating a Web application using ASP.Net MVC as the core framework "No punn intended". 
The "My Commerce App" allows users to sign in and view their bank account infromation. 
This application was designed to allow the user to create custom alerts to grant them better access and control of their bank account.
It consists of A Login Page, Account Summary Page, Transaction Summary Page, Alrets Page, and Manage Alerts page.


I highly reccomend running this applicaiton using Visual Studio 2015, 2017, or 2019. This is the IDE used to develope this application.

Using the Correct NuGet packages is important, that is why I reccomend using VS. If you have the .Net core packeges installed (using C#)
then most of the NuGet dependincies will already be available. For a full representation of required NuGet packages pleas view the Web.config file in the repo.

These NuGet packages MUST be inlcude:
--> Entity PagedList by Troy Goode (PagedList.MVC will also work)
--> System.Data.SQLite (or System.Data.SQLite.Core)
--> System.Data.SQLite.Linq
--> EPP.Plus (For exporting xml files)
--> OfficeOpenXML.Extends
--> DocumentFormat.OpenXML
--> Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.1
--> Microsoft.AspNet
--> bootsrap 3.0.0

Getting Set Up:

This is a simulated online bank transaction application using a SQLite DB that has been auto populated. 
To access this application, clone this repo and add the appropriate NuGet packeges. 
When the repo is cloned a CommerceDB.db file will also be included. It it crucial that you update the CommerceDB.db file location in the Web.config file found on //Line 16

What to expect:

This is a rough experiment to help me learn the ASP.Net framework. 
When you run the appliaction you will first be asked to login. (No information is required so feel free to click Login)
Then you will see some account summary choices. Choose an accout and you will be directed to that Accounts Transaction Summary page
The transaction summary page allows you to view your current ballance, and displays all transactions in a table format. 
The table/transactions can be filtered, searched, and the table its self can be viewd in multiple way
Next, you may view your Alerts, which are custom tools used to track you accounts behavior so that you can detect any unwanted or 
cponcerning transactions right away.
Manage Alerts will allow you to set new custom alerts based on your individul needs. 
