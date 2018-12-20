# ConfigSplitter
Splite elements of app.config or web.config to multiple files

If you have one web app for example surving more than one client. everytime you want to update the application you have to take care about the web.config because it contains the connection strings which is differ for each client.
the easy solution for that is to make the main web.config contains a pointer for another file which contains the connection string as following
<connectionStrings configSource="connectionStrings.config" />

and the other file which contains the connection string as following

<?xml version="1.0" encoding="utf-8"?>
<connectionStrings>
 <add name="Reports.ar.Properties.Settings.ERP" connectionString="Data Source=Server;Initial Catalog=Database;Persist Security Info=True;User ID=User;Password=P@ssw0rd;" providerName="System.Data.SqlClient" />
 </connectionStrings>

this code is a tool to select the web.config or app.config file and then split the sections you want in other files bu modifing the _elementPaths variable for required sections

I put the implmemntation code using functional programing style and Imperative programing style just for fun :)
