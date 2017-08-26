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
The implementation of the solution should not be a monolithic project. In order to apply the separation of concerns multiple projects have been introduced, using consistent naming conventions
| Project Name  | Description |
| ------------- | ------------- |
| Xero.AspNet.Core | Core classes that are shared among all projects |
| Xero.Refactor.Data | EF, migration and Repositories |
| Xero.Refactor.Data.Models | EF models |
| Xero.Refactor.Services | Services |
| Xero.Refactor.WebApi | The Web API |
| Xero.Refactor.WebApi.Modeling | API models |
