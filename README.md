# 🛒 Prokoders E-Commerce Application

## 📋 Overview

**Prokoders E-Commerce** is a modern, full-featured online store built with:

- ASP.NET Core 6.0 MVC
- Entity Framework Core
- SQL Server

It supports:

- User authentication
- Product management
- Shopping cart
- Real-time updates using SignalR
- A complete checkout system

---

## ⚙️ Setup Instructions

### 🧰 Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Docker](https://www.docker.com/)
- Visual Studio 2022+ **or** Visual Studio Code

---

## 💻 Local Development (Without Docker)

### 1. Configure `appsettings.json`

Update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProkodersECommerceDB;User Id=sqlserverauth;Password=admin;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### Local Development (Without Docker)

1. **Ensure SQL Server is running** and the `sqlserverauth` user has `db_owner` access.

2. **Apply Migrations & Seed Database:**
   ```bash
   dotnet ef database update
   ```
2. **Run the application:**
    ```bash
   dotnet run
   ```
    or you can run it normally using Visual Studio IDE

   ### Run with Docker in Visual Studio 2022
   ## ✅ Prerequisites

- **Docker Desktop** is installed and running.
- **Visual Studio 2022** with the following workloads:
  - ASP.NET and web development  
  - .NET Core cross-platform development
- Ensure your project includes a `Dockerfile` and a `docker-compose.yml`.

---

## ▶️ Steps

1. **Open the solution** in Visual Studio 2022.

2. In **Solution Explorer**, right-click on the project and choose:  
   `Add > Docker Support`  
   - Choose **Linux** or **Windows containers** depending on your Docker setup.
   - This will generate a `Dockerfile` in your project and optionally add a `docker-compose` project to the solution.

3. **Right-click the `docker-compose` project** (if added) and set it as the **Startup Project**.

4. Open the **Run dropdown** (next to the green play button), and select:  
   `docker-compose`

5. **Run the project**:  
   Press `F5` or click the green **Run** button to build and start the containers.

---

## ⚠️ Notes

- Visual Studio will automatically handle:
  - Container build
  - Volume mapping
  - Port forwarding

- You can **debug the application inside the container** with breakpoints as usual.

- If you're using **SQL Server locally**:
  - Make sure it's running and accessible from the container (e.g., via `host.docker.internal` for Windows containers).
  - Ensure **SQL Authentication** is enabled.


   ### Running with Docker
   1. **Ensure SQL Server is running locally and accepting SQL Authentication on port 1433.
  
   2. **Run containers:
      ```bash
       docker-compose up --build
      ```

      ## Design Decisions

### Technologies Used

| Area             | Technology                      |
|------------------|----------------------------------|
| Backend          | ASP.NET Core 6.0 MVC             |
| ORM              | Entity Framework Core            |
| Database         | SQL Server                       |
| Real-Time Comm   | SignalR                          |
| Frontend         | Razor Views, Bootstrap           |
| Auth             | ASP.NET Core Identity            |
| Deployment       | Docker + Docker Compose          |

---

### Architectural Patterns

- **Repository Pattern**: Abstracts data access logic.
- **Factory Pattern**: Centralizes complex object creation (e.g., for Orders).
- **Singleton Pattern**: Used for shared services like `EmailService`.
- **Observer Pattern**: Implemented using SignalR for real-time cart updates.

---

## Features

- Product listing, filtering, and pagination  
- Admin CRUD for products  
- Shopping cart with real-time updates (SignalR)  
- Order placement and confirmation  
- Email notification service (SMTP)  
- Role-based access control  
- Dockerized for deployment  
