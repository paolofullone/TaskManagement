# Task Management GraphQL API

## Overview
This project is a .NET GraphQL API for managing users, projects, and tasks. Built with HotChocolate for GraphQL and Entity Framework Core for database operations, it uses a SQL Server database running in a Docker container. The API supports advanced querying with filtering, sorting, and pagination, making it ideal for handling large datasets efficiently.

### Features
- **GraphQL Queries**: Retrieve users, projects, and tasks with flexible filtering, sorting, and pagination.
- **Database**: SQL Server with a schema for `Users`, `Projects`, and `WorkTasks`.
- **Data Models**: Well-defined entities with relationships (e.g., tasks linked to users and projects).
- **Dockerized Environment**: Includes SQL Server and MSSQL tools for easy database setup.
- **Sample Data**: Pre-populated with users, projects, and tasks for testing.

## Project Structure
- **`docker-compose.yaml`**: Configures SQL Server and MSSQL tools for database initialization.
- **`Query.cs`**: Defines GraphQL query resolvers for retrieving data.
- **`ApplicationDbContext.cs`**: Entity Framework Core context mapping entities to database tables.
- **`User.cs`, `WorkTask.cs`, `Project.cs`**: C# models defining the data structure.
- **`database.sh`**: Script to execute SQL initialization.
- **`init.sql`**: SQL script to create and seed the `TaskManagementDB` database.

## Data Models
- **User**: Represents a user with properties:
  - `Id`: Unique identifier (int).
  - `Name`: User’s name (string).
  - `Email`: Unique email address (string).
  - `Tasks`: Collection of associated tasks (List<WorkTask>).
- **Project**: Represents a project with properties:
  - `Id`: Unique identifier (int).
  - `Name`: Project name (string).
  - `Description`: Project description (string).
  - `Tasks`: Collection of associated tasks (List<WorkTask>).
- **WorkTask**: Represents a task with properties:
  - `Id`: Unique identifier (int).
  - `Title`: Task title (string).
  - `Description`: Task description (string).
  - `IsCompleted`: Completion status (boolean).
  - `UserId`: Foreign key to `User` (int).
  - `User`: Associated user (navigation property).
  - `ProjectId`: Foreign key to `Project` (int).
  - `Project`: Associated project (navigation property).

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install)
- A GraphQL client (e.g., [Banana Cake Pop](https://chillicream.com/docs/hotchocolate/get-started) or Postman)

## Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Configure Environment Variables (optional)**
   - Create a `.env` file in the root directory:
     ```env
     SQL_SA_PASSWORD=YourSecurePassword123!
     ```
   - Ensure the password meets SQL Server complexity requirements (at least 8 characters, including uppercase, lowercase, numbers, and symbols).

3. **Start Docker Services**
   ```bash
   docker-compose up -d
   ```
   - This starts:
     - `sqlserver`: SQL Server 2022 on port `1433`.
     - `mssqltools`: Executes `init.sql` to create and seed the `TaskManagementDB` database.
   - Wait ~30 seconds for initialization or check logs:
     ```bash
     docker logs mssqltools
     ```

4. **Configure the .NET API**
   - Update `appsettings.json` with the connection string:
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Server=localhost,1433;Database=TaskManagementDB;User Id=sa;Password=${SQL_SA_PASSWORD};TrustServerCertificate=True;"
       }
     }
     ```
   - Restore dependencies:
     ```bash
     dotnet restore
     ```

5. **Run the API**
   ```bash
   dotnet run --project src/WebApi
   ```
   The API is available at `https://localhost:<port>/graphql` (or `http` in development).

6. **Test the API**
   Use a GraphQL client (like hotchocolate) to execute queries. Below are example queries showcasing the API’s capabilities:

![GraphQL Query Examples](./images/hotchocolate.png)

   **Get Project Names**
   ```graphql
   query {
     projects {
       nodes {
         name
       }
     }
   }
   ```

   **Get Paginated Users with Tasks**
   ```graphql
   query {
     users {
       nodes {
         name
         tasks {
           title
         }
       }
       pageInfo {
         hasNextPage
       }
     }
   }
   ```

   **Get User by ID with Tasks and Projects**
   ```graphql
   query {
     userById(id: 1) {
       name
       tasks {
         title
         project {
           name
         }
       }
     }
   }
   ```

   **Get Completed Tasks**
   ```graphql
   query {
     completedTasks(isCompleted: true) {
       nodes {
         title
         user {
           name
         }
       }
     }
   }
   ```

   **Get Tasks for a Specific User**
   ```graphql
   query {
     tasksByUser(userId: 2) {
       nodes {
         title
         isCompleted
       }
     }
   }
   ```

   **Get Finished Tasks for a User**
   ```graphql
   query {
     filteredTasksByUser(userId: 1, isCompleted: true) {
       nodes {
         id
         title
         isCompleted
         project {
           name
         }
       }
       pageInfo {
         hasNextPage
         endCursor
       }
     }
   }
   ```

## Database Schema
- **USERS**: `Id` (PK), `Name`, `Email` (unique).
- **PROJECTS**: `Id` (PK), `Name`, `Description`.
- **WORKTASKS**: `Id` (PK), `Title`, `Description`, `IsCompleted`, `UserId` (FK to `USERS`), `ProjectId` (FK to `PROJECTS`).

## Sample Data
- **Users**: Paolo, Kely, Luca, Manu.
- **Projects**: Website Redesign, Mobile App.
- **Tasks**: 20 tasks assigned to users across projects, with varying completion statuses (e.g., "Design Homepage", "Implement Auth").

## Security Notes
- **SQL Server Password**: Use a strong password for `SQL_SA_PASSWORD` in production and store it securely (e.g., in a secrets manager or `.env` file).
- **HTTPS**: Enable HTTPS in production by configuring Kestrel or using a reverse proxy (e.g., NGINX).
- **Sensitive Data**: Avoid exposing sensitive data in GraphQL error messages.

## Troubleshooting
- **Database not initialized**:
  - Check `mssqltools` logs:
    ```bash
    docker logs mssqltools
    ```
  - Verify SQL Server is running:
    ```bash
    docker ps
    ```
  - Test database connection:
    ```bash
    docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SQL_SA_PASSWORD -Q "SELECT * FROM TaskManagementDB.dbo.USERS"
    ```
- **GraphQL errors**:
  - Inspect the schema using a GraphQL IDE (e.g., Banana Cake Pop).
  - Ensure queries match the schema (e.g., `userById` expects an `id` argument).
- **No data returned**:
  - Verify seed data in `init.sql` was applied.
  - Test with queries like `userById(id: 1)` or `filteredTasksByUser(userId: 1, isCompleted: true)`.


