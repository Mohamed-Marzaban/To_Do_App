# To-Do Web API

A simple To-Do list Web API built with ASP.NET Core and Entity Framework Core. This project demonstrates RESTful endpoints for user authentication (JWT), task CRUD operations, and basic authorization using claims.

---

## Features

* User registration and login (JWT authentication)
* Add, update, delete, and list to-do tasks
* Per-user task ownership and authorization
* Passwords are hashed using BCrypt and securely stored
* Automatic database migrations on app startup
* Example of adding claims (username and id) to JWT
* Swagger documentation available for API endpoints

---

## Tech stack

* .NET 8/9 (adjust SDK target as needed)
* ASP.NET Core Web API
* Entity Framework Core (EF Core)
* JWT Bearer Authentication
*  Mysql (configurable via `appsettings.json`)
* Swagger (OpenAPI) for API documentation

---

## Prerequisites

* .NET SDK (8.0 or 9.0 depending on your project target)
* A database server
* `dotnet-ef` tool (optional, for migrations):

```bash
dotnet tool install --global dotnet-ef
```

---

## Setup

1. **Clone the repository**

```bash
git clone <repo-url>
cd To_Do_Web_Api
```

2. **Restore dependencies**

```bash
dotnet restore
```

> If you see an error like `NETSDK1045: The current .NET SDK does not support targeting .NET 9`, either install the appropriate SDK or change the target framework in the `.csproj` (e.g. to `net8.0`).

3. **Configure the database**

* Edit `appsettings.json` to point to your connection string (SQL Server or SQLite).
* Example (SQL Server):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ToDoDb;Trusted_Connection=True;"
}
```

4. **Automatic migrations**

The application applies any pending EF Core migrations automatically on startup. No manual migration command is needed.

5. **Set JWT settings**

In `appsettings.json` add your JWT secret and other settings:

```json
"Jwt": {
  "Key": "your-very-strong-secret-key",
  "Issuer": "YourApp",
  "Audience": "YourAppUsers",
  "ExpiresInMinutes": 60
}
```

---

## Run

```bash
dotnet run
```

By default the API should run on `https://localhost:5001` (or a port printed in the console). Swagger documentation is available at `/swagger`.

---

## Authentication & JWT

* Register a user: `POST /api/auth/register` (body: `username`, `password`)
* Login: `POST /api/auth/login` (body: `username`, `password`) → returns JWT token

### Adding claims to JWT

When creating the token, it's common to include claims such as the user's id and username. Example claim construction:

```csharp
var claims = new[] {
    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
    new Claim("id", user.Id.ToString()),
    // role or other claims if needed
};
```

Then sign the token as usual. When validating requests, you can read these claims from the `User` principal (e.g. `User.FindFirst("id")?.Value`).

---

## API Endpoints (example)

> Replace base URL with your running host (e.g., `https://localhost:5001`).

### Auth

* `POST /api/auth/register` — register new user

  * Body: `{ "username": "alice", "password": "P@ssw0rd" }`
* `POST /api/auth/login` — login

  * Body: `{ "username": "alice", "password": "P@ssw0rd" }`
  * Response: `{ "token": "<jwt>" }`

### Tasks (authorized)

* `GET /api/tasks` — list tasks for the authenticated user
* `GET /api/tasks/{id}` — get single task (must belong to user)
* `POST /api/tasks` — create a task

  * Body example: `{ "title": "Buy milk", "description": "2 liters" }`
* `PUT /api/tasks/{id}` — update a task (must belong to user)
* `DELETE /api/tasks/{id}` — delete a task (must belong to user)

Use the `Authorization: Bearer <token>` header for all protected endpoints.

---

## Security Concerns

* Always hash passwords (this project uses BCrypt).
* Use a long, random JWT secret stored in environment variables or a secret manager.
* Set appropriate token expiration and support refresh tokens if needed.
* Validate ownership server-side for every task operation (compare `task.UserId` with the authenticated user id).

