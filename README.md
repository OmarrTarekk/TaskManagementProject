# TaskManagement
Task Management RESTful API built with ASP.NET Core 8 following Clean Architecture principles.

## Technologies

* ASP.NET Core 8
* Entity Framework Core
* SQL Server
* AutoMapper
* xUnit
* FluentAssertions
* Swagger

## Project Structure

* TaskManagement.Api
* TaskManagement.Application
* TaskManagement.Domain
* TaskManagement.Infrastructure
* TaskManagement.Tests.Integration

## Prerequisites

* .NET 8 SDK
* SQL Server

## Setup

1. Clone the repository:git clone https://github.com/OmarrTarekk/TaskManagementProject.git
2. Update the connection string in `appsettings.json`.

3. Apply migrations:
4. Run the API:
5. Open Swagger:https://localhost:<port>/swagger
## Features

* CRUD for Projects
* CRUD for Tasks
* Pagination
* Filtering
* Sorting
* Search
* Integration Tests

## API Endpoints

### Projects

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/projects` | Create a project |
| GET | `/api/projects` | List all projects (paginated) |
| GET | `/api/projects/:id` | Get a single project |
| PUT | `/api/projects/:id` | Update a project |
| DELETE | `/api/projects/:id` | Delete a project (cascades tasks) |

### Tasks

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/projects/:id/tasks` | Create a task under a project |
| GET | `/api/projects/:id/tasks` | List tasks for a project |
| GET | `/api/tasks/:id` | Get a single task |
| PUT | `/api/tasks/:id` | Update a task |
| DELETE | `/api/tasks/:id` | Delete a task |
| GET | `/api/tasks` | List all tasks |

### Example: Create Project

```http
POST /api/projects
Content-Type: application/json

{
  "name": "My Project",
  "description": "Optional description"
}
```

### Example: Create Task

```http
POST /api/projects/1/tasks
Content-Type: application/json

{
  "title": "My Task",
  "status": "todo",
  "priority": "medium",
  "due_date": "2025-12-31"
}
```

### Query Parameters (Tasks)

| Parameter | Description |
|-----------|-------------|
| `q` | Search in title and description |
| `status` | Filter: `todo`, `in_progress`, `done` |
| `priority` | Filter: `low`, `medium`, `high` |
| `due_date_from` | Filter from date |
| `due_date_to` | Filter to date |
| `sort` | `due_date`, `priority`, `created_at` |
| `order` | `asc` or `desc` |
| `page` | Page number |
| `limit` | Items per page |

## Architecture

The project follows Clean Architecture using Repository Pattern and Unit of Work.
