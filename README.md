# Task Management System

This solution contains:
- **TaskManager.Api**: ASP.NET Core Web API (.NET 8, in-memory EF Core, CRUD for Task entity)
- **TaskManager.Blazor**: Blazor Server frontend (Bootstrap 5, task management components)

## Getting Started

1. **Restore dependencies:**
   ```bash
   dotnet restore
   ```
2. **Build the solution:**
   ```bash
   dotnet build
   ```
3. **Run the API:**
   ```bash
   dotnet run --project TaskManager.Api
   ```
4. **Run the Blazor frontend:**
   ```bash
   dotnet run --project TaskManager.Blazor
   ```

## Features
- Task CRUD (Create, Read, Update, Delete)
- Responsive UI with Bootstrap 5
- In-memory database (no setup required)
- API error handling and validation
- Blazor components for task management

---

For more details, see the source code and comments.
