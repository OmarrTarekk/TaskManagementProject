# TaskManagement

Task Management RESTful API built with ASP.NET Core 8 following Clean Architecture principles.

## Technologies

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- AutoMapper
- xUnit
- FluentAssertions
- Swagger

## Project Structure

- TaskManagement.Api
- TaskManagement.Application
- TaskManagement.Domain
- TaskManagement.Infrastructure
- TaskManagement.Tests.Integration

## Prerequisites

- .NET 8 SDK
- SQL Server

## Setup

1. Clone the repository:

```bash
git clone https://github.com/OmarrTarekk/TaskManagementProject.git
```

2. Update the connection string in `appsettings.json`.

3. Apply migrations:

```bash
dotnet ef database update
```

4. Run the API:

```bash
dotnet run --project TaskManagement.Api
```

5. Open Swagger:

```
https://localhost:<port>/swagger
```

## Features

- CRUD for Projects
- CRUD for Tasks
- Pagination
- Filtering
- Sorting
- Search
- Integration Tests

## Architecture

The project follows Clean Architecture using Repository Pattern and Unit of Work.
