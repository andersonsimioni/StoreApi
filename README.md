# .NET CORE 6 REST API

This REST Api have 3 CRUDs for Customers, Items and Orders, also it have a code first approach for Entity Framework, the data layer is separated in 2 modules named
as DataRepository and DataExtensions, the DataRepository module contains all models object of the database, migrations and auto mapper profiles, there is no DML 
methods. In DataExtensions we have the DML methods for access the database entities, this module use a new approach to integrate extensions into the DbContext, so
all methods will be extended into the DbContext object when use it. Another important folder is the Shared folder, it contains a module named Settings that share 
the software configuratiosn such as string connections through all common projects in the solution. Also the Shared folder contains the modules Services and Structs to
share more entities through the projects. And finally, our REST API is in the WebApi folder, this API runs in .NET CORE version 6.