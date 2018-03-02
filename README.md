# country-code-api
**Overview**

A goal of the project is to provide a RESTful API to some useful information about countries and currencies.


**Endpoints**

Countries

1) Country name and code (ISO 3166-1 alpha-2).
2) Currencies in circulation in the country.
3) International organizations, zones, and areas, which includes the country.
4) Date format and an international country phone code.

Currencies

1) Currency name and code (ISO 4217).
2) Countries in which the currency is official.

International organization, zones, and areas.

1) Short name and description.
2) Countries in the organization/zone/area.


**Why may you need it**

You can use it for it's intended purpose: set up and deploy the service in your company's infrastructure.

Or use the project as a template for developing a Web-service based on ASP.NET Web API.


Also, you can take this project as an example of usage of different approaches and technologies:

* Web backend: RESTful API, ASP.NET Web API 2.2. JSON as request/response format. Generation of Help pages using 
Swagger/Swashbuckle. 

* Dependency injection applied through Ninject.

* Pipeline: OWIN/Katana.

* Authentication and authorization: OAuth, Identity. Read access is open for everyone, CRUD is only for authorized users.

* Data: Entity Framework, Code First, Migrations. Repository and Unit of Work design patterns are used to decouple Data 
layer and Web API code. So when you are going to change DBMS, ORM-framework, or that guys from the EF team have rewritten 
everything again, you will not have troubles.

* Testing: Unit testing with TestTools.UnitTesting, Moq. 

All this is put together, set up, checked and working. Most of the code covered by Unit tests.


**Content of the project**

At this moment, the project contains all the code needed to compile and deploy the service, as well as Code First migrations 
to create the database. So you receive a functional Web-service with ready-to-use API and database, but without data itself. 
All that is remaining to do is to fill the database, set up connection string in the config file, set up HTTPS in your 
Web-server.
