[[_TOC_]]

# ASP .NET Core Web API Template
This template for creating ASP .NET Core Web API Template was crated for the Monthly Engineering Workshops at hack3rlife.

# How the code is organized
The solution is organized in the following way

    - CleanArchitecture.WebApi.sln

        | - README.md

        | - src

            | - CleanArchitecture.Application

            | - CleanArchitecture.Domain

            | - CleanArchitecture.Infrastructure

            |- CleanArchitecture.WebApi

        | - tests

            | - CleanArchitecture.Application.UnitTest

            | - CleanArchitecture.Infra.IntegrationTests

            | - CleanArchitecture.WebApi.EndToEndTests


## Domain Project
This project will include Domain Models, Interfaces that will be implemented by the outside layers, enums, etc., specific to the domain logic.  This project should not have any dependecy to another project since it is the core of the project.

### Domain Types
* Domain Models (e.g. `Blog`)
* Interfaces (e.g. `IBlogRepository`)
* Exceptions
* Enums

### Application Project
This project will contain the application logic. The only dependency that should have is the Domain project. Any other project dependency must be removed.

### Appllication Types
* Pipeline Behaviors (eg.: `ValidatorBehavior`)
* Exceptions (e.g.: `ApplicationValidationException`)
* Commands Handlers (e.g.: `AddBlogRequestCommandHandler`)
* Query Handelrs (e.g.: `GetBlogsRequestQueryHandler`)
* DTOs (e.g.: `AddBlogRequestCommand`)
* Mappers (e.g: `ProfileMapper`)

### Infrastructure Project
The Infrastructure project generally includes data access implementations or accessing external resources as file sytems, SMTP, third-party services, etc.  These classes should implementations of the Interfaces defined in the Domain Project.  Therefore, the only dependency in this project should be to the Domain Project.  Any other dependency must be removed.

### Infrastructure Types
* EF Core Types (e.g.: `BlogDbContext`) 
* Repository Implementation (e.g.: `BlogRepository`)

### WebAPI Project
This is the entry point of our application and it depends on the Application and Infrastrucre projects.  The dependency on Infrastructre Project is requiered to support Dependency Injection in the `Startup.cs` class.  Therefore, no direct instantiation of or static calls to the Infrastucture project should be allowed.

### WebAPI Types
* Controllers (e.g.: `BlogsController`)
* Filters (e.g.: `CustomExceptionFilterAttribute`)
* Startup
* Program 