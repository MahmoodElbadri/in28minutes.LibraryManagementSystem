```markdown
# In28Minutes.Library

This project is a library management system that demonstrates the use of modern .NET Core web API development practices.

---

## Features

### **Core Functionalities**
- **User Authentication**:
  - Secure login and registration endpoints through the `AuthController`.
- **Book Management**:
  - CRUD operations for books via the `BooksController`.
- **Category Management**:
  - Manage categories associated with books through the `CategoriesController`.
- **User Management**:
  - Administrative functions for managing users through the `UsersController`.

### **Middleware**
- **Exception Handling Middleware**:
  - Provides centralized error logging using Serilog and ensures meaningful error responses.

### **Database Management**
- **Entity Framework Core**:
  - `ApplicationDbContext` for database interaction.
  - Migrations are managed for schema changes.
- **Seed Data**:
  - Preloads the database with necessary data using `SeedDataExtension`.

### **Repository Pattern**
- Follows the repository pattern for abstracting data access:
  - `BookRepository`, `CategoryRepository`, and `UserService` handle core database operations.
  - `UnitOfWork` ensures transactional integrity.

### **Automated Mapping**
- **AutoMapper**:
  - Simplifies object-to-object mapping for DTOs and entities with mapping profiles defined in `Mappings/Mapping.cs`.

---

## Project Structure

### **Controllers**
- **`AuthController`**: Manages user authentication and registration.
- **`BooksController`**: Handles CRUD operations for books.
- **`CategoriesController`**: Manages book categories.
- **`UsersController`**: Administrative endpoints for user management.

### **Data**
- **`ApplicationDbContext`**: The EF Core database context for managing database interactions.
- **Seed Data**: Extension methods to populate the database with initial data.

### **Middlewares**
- **`ExceptionHandlingMiddleware`**: Handles exceptions globally and logs errors using Serilog.

### **Models**
- **`Book`**: Represents book entities.
- **`Category`**: Represents book categories.
- **`User`**: Represents user accounts and roles.

### **Repositories**
- **Interfaces**: Define contracts for database operations:
  - `IBookRepository`, `ICategoryRepository`, `IUserService`.
- **Implementations**: Provide actual database handling:
  - `BookRepository`, `CategoryRepository`.
- **`UnitOfWork`**: Ensures transactional integrity.

---

## Technology Stack

- **.NET Core 8**: Backend framework for building RESTful APIs.
- **Entity Framework Core**: ORM for managing database interactions.
- **AutoMapper**: For efficient mapping of models to DTOs.
- **SQL Server**: Relational database for data persistence.

---

## Getting Started

### Prerequisites
1. **Install .NET 8 SDK**.
2. Set up **SQL Server** for the database.

### Steps
1. Clone the repository:


2. Configure your connection string in `appsettings.json`:


3. Apply migrations and seed data:


4. Run the application:


---

## Contributions

Contributions are welcome! Feel free to fork this repository and submit a pull request for any improvements.

---
