# Todo Project

A simple Todo application built with .NET 8 and ASP.NET Core, featuring REST API endpoints for managing todo items.

## Project Structure

- **Todo.Api** - ASP.NET Core Web API with Todo endpoints
- **Todo.AppHost** - Aspire AppHost for orchestration (requires Aspire workload)
- **Tests1** - Integration tests for the Todo API

## Features

- ✅ Create new todo items
- ✅ Get all todo items
- ✅ Get a specific todo item by ID
- ✅ Update existing todo items
- ✅ Delete todo items
- ✅ Mark todos as completed/uncompleted

## API Endpoints

- `GET /todos` - Get all todo items
- `GET /todos/{id}` - Get a specific todo item
- `POST /todos` - Create a new todo item
- `PUT /todos/{id}` - Update an existing todo item  
- `DELETE /todos/{id}` - Delete a todo item

## Getting Started

### Prerequisites

- .NET 8.0 SDK

### Running the API

```bash
cd Todo.Api
dotnet run
```

The API will be available at `http://localhost:5290` (or the port shown in the console).

### Running Tests

```bash
dotnet test
```

### Building the Solution

```bash
dotnet build
```

## Example Usage

### Create a Todo

```bash
curl -X POST http://localhost:5290/todos \
  -H "Content-Type: application/json" \
  -d '{"Title": "Learn .NET", "Description": "Study ASP.NET Core"}'
```

### Get All Todos

```bash
curl http://localhost:5290/todos
```

### Update a Todo

```bash
curl -X PUT http://localhost:5290/todos/1 \
  -H "Content-Type: application/json" \
  -d '{"Title": "Learn .NET", "Description": "Study ASP.NET Core", "IsCompleted": true}'
```

## Data Models

### TodoItem
- `Id` (int) - Unique identifier
- `Title` (string) - Todo title
- `Description` (string, optional) - Todo description
- `IsCompleted` (bool) - Completion status
- `CreatedAt` (DateTime) - Creation timestamp

### CreateTodoRequest
- `Title` (string) - Todo title
- `Description` (string, optional) - Todo description

### UpdateTodoRequest
- `Title` (string) - Todo title
- `Description` (string, optional) - Todo description
- `IsCompleted` (bool) - Completion status

## Architecture

The application uses:
- **ASP.NET Core Minimal APIs** for REST endpoints
- **In-memory storage** for simplicity (todos are lost on restart)
- **Dependency injection** for service management
- **Integration testing** with WebApplicationFactory
- **Swagger/OpenAPI** for API documentation (available at `/swagger` in development)