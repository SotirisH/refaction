# Implementation Notes

## Overview
The project is a Web API and we will try to apply the best practices & guidelines as instructed by major companies on the market such as the Microsoft, GitHub and ebay.
The purpose is to create a web API that for the developer's point of view should have the following characteristics: 
- The code is simple, well written and clean
- Contains enough amount of comments that will help any future developer do understand and maintain it
- Follows the SOLID Principles
- Ensures the quality by implementing various unit and integration tests
 
For the consumers, the following principles should be followed:
- Provide the smoothest possible experience for developer’s consumers
- Follow consistent design guidelines to make using them easy and intuitive
- Provide clear type results and meaningful error messages
- Enable navigation by using HATEOAS approach
- Generating good documentation and help pages as a part of your Web API using Swagger
 
## Creating project structure
The implementation of the solution should not be a monolithic project. In order to apply the single rensponsibility and Segregation of concerns principles, multiple projects have been introduced, using consistent naming conventions

| Project Name | Description |
| --- | --- |
| Xero.AspNet.Core | Core classes that are shared among all projects |
| Xero.Refactor.Data | EF, migration and Repositories |
| Xero.Refactor.Data.Models | EF models |
| Xero.Refactor.Services | Services |
| Xero.Refactor.WebApi | The Web API |
| Xero.Refactor.WebApi.Modeling | API models |
| Xero.Refactor.Test.Common| Fixtures |
| Xero.Refactor.WebApiTests | Tests |
| Xero.Refactor.ServicesTests | Integration Tests |


## Implementing Core services
The core services include 
-	a Generic repository
-	a custom unit of work
-	An auditable DbContext
These, I have implemented already in one of my demo projects https://github.com/SotirisH/MVC

## Setup Entity framework & models
The main dbcontext and its migrations will live in the Xero.Refactor.Data. 
The models are created into the Xero.Refactor.Data.Models and represent the db structure where their configuration exist in the file ModelConfigurations
As the database exists why don’t want to destroy the existing schema or data we follow the steps as described here https://msdn.microsoft.com/en-us/library/dn579398(v=vs.113).aspx
Steps:
-	Create RefactorDb
-	Enable migrations with the command Enable-Migrations
-	Create Initial migration
-	Update DB

As we finished with the setup the next step is to enhance the models to support Audit and optimistic concurrency using timestamps



## Implement the services
All the services are implemented to run async. Dtos are usually returned by the services instead of EF Models. It is proven as the project it grows bigger and more complex the DTOs look less and less similar to the EF Models and are the best way to transfer the data between the layers.
I have spent huge time investigating if the repository pattern is necessary with EF as the last implements the repository and unit of work pattern This debate is start getting so old as the int ID vs Guid debate. Despite, my final conclusion is that it is not necessary to create another abstraction on the top of the EF, for this project I will implement one just to show on how a generic repository can be implemented. Although in my .net Core implementation, I use directly the ef. 
A lot of discussion is [here]( http://www.thereformedprogrammer.net/is-the-repository-pattern-useful-with-entity-framework/)
Introducing Automapper: Automapper will help us with the transformation of the models among the layers.
Another principle here is that each method represents a business transaction thus the “savechanges” is called at the end of the method when it is needed.
All services are tested using integration tests against a memory Database.

## Implement the controller
- Phase 1-Modeling
- Introducing https://github.com/JeremySkinner/fluentvalidation
- Add unity
- Map webApiModels with the DTOs 
- The controller actions are covered by unit test as we need only to check the return result and the login in the controller method
- Each action runs Asynchronous in order to release the application pool for other requests when a long I/O process takes place and returns correct Http Status codes. 
## Implement global Error handling
A global error handler is essential to trap all the unhandled errors and return a meaningfull result to the client. An ErrorResult is returned in this case.

## Apply HATEOAS principle
The HATEOAS approach enables a client to navigate and discover resources from an initial starting point. This is achieved by using links containing URIs; when a client issues an HTTP GET request to obtain a resource, the response should contain URIs that enable a client application to quickly locate any directly related resources

## Use Swagger for a neat documentation
Swagger is a very cool tool. By adding XML comments offers a quick documentation for other developers and raises the image of the company to look very professional
## Additional features that could be implemented
-	Add Security
-	Implement Paging & filters
-	Implement full HATEOAS, including navigation through the pages
-	…

# Implementation Notes (ASP.NET Core)

## Overview
The ASP.NET Core is the technology of the future and worth investing on it. The Core implementation is similar to the traditional one with one notable difference: There is no use of an additional Repository or unit of work as EF implements them both.
For testing the SQL Lite provider is used where the database is created in the memory
I created an new branch here https://github.com/SotirisH/refaction/tree/ASP.Net_Core
