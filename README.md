
# Task Management API

A simple API built with .NET Core 6.0 for managing tasks. This project demonstrates CRUD operations, in-memory database usage, error handling, and logging.

---

## Features
-- Deployed Api on Azure (below is the link for the same)
-- Custom swagger Response 
- CRUD operations for task management.
- In-memory database for lightweight testing.
- Optional filters for fetching tasks:
  - **Status**: Pending, In Progress, Completed.
  - **Due Date**: Fetch tasks due before a specific date.
- Pagination support for large datasets.
- Logging for key events (e.g., task creation, updates, deletions).
- Proper error handling with meaningful messages and HTTP status codes.
- Swagger UI for interactive API documentation.

---

## Technologies Used

- **Framework**: .NET Core 6.0
- **Language**: C#
- **Database**: In-memory (Entity Framework Core)

---

## Getting Started

### Azure Api Link
```
https://apitaskmanagement.azurewebsites.net/index.html
```

### Setup Instructions

1. **Clone the Repository**:
   ```bash
   git clone <repository-url>
   cd TaskManagementApi
   ```

2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the Project**:
   ```bash
   dotnet build
   ```

4. **Run the API**:
   ```bash
   dotnet run
   ```

5. **Access the API**:
   - By default, the API is available at:
     - `http://localhost:5000`
     - `https://localhost:5001`

6. **Swagger UI**:
   - Visit the interactive API documentation at:
     ```
     http://localhost:5000/swagger
     ```

---

## API Endpoints

### 1. **GET /tasks**
- Fetch all tasks.
- Optional filters:
  - `status` (Pending, In Progress, Completed)
  - `dueDate` (tasks due before a specific date)
- Supports pagination with `pageNumber` and `pageSize`.

### 2. **GET /tasks/{id}**
- Fetch details of a specific task by ID.

### 3. **POST /tasks**
- Create a new task.
- Request body example:
  ```json
  {
    "title": "Sample Task",
    "description": "Task description",
    "status": "Pending",
    "dueDate": "2024-12-15T10:00:00"
  }
  ```

### 4. **PUT /tasks/{id}**
- Update an existing task by ID.
- Request body example:
  ```json
  {
    "title": "Updated Task",
    "description": "Updated description",
    "status": "In Progress",
    "dueDate": "2024-12-20T15:00:00"
  }
  ```

### 5. **DELETE /tasks/{id}**
- Delete a task by ID.

---

## Bonus Features

1. **In-Memory Database**:
   - Simplifies local testing without additional setup.

2. **Error Handling**:
   - Returns appropriate HTTP status codes:
     - `404 Not Found` for missing resources.
     - `400 Bad Request` for validation errors.
     - `500 Internal Server Error` for unexpected issues.

3. **Logging**:
   - Logs key events to aid debugging and monitoring.

4. **Swagger Integration**:
   - Interactive documentation for easier testing and exploration.

---

## Future Enhancements

- Add authentication and authorization.
- Support exporting tasks as CSV.
- Integrate with a persistent database like SQL Server for production.

---
