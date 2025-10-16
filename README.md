# PRN232 Lab 2 - Coffee Store Management System

This is a REST API for a Coffee Store management system built with ASP.NET Core following a three-layer architecture.

## Project Structure

- **PRN232.Lab2.CoffeeStore.API**: The main API project containing controllers and startup configuration
- **PRN232.Lab2.CoffeeStore.Services**: Business logic layer with service implementations
- **PRN232.Lab2.CoffeeStore.Repositories**: Data access layer with Entity Framework Core

## Features

- JWT-based Authentication with Access Token and Refresh Token
- Content Negotiation (JSON/XML)
- Generic Repository and Unit of Work patterns
- Advanced GET endpoints with search, sort, paging, and field selection
- Full CRUD operations for all entities
- Role-based Authorization (Admin/Staff)

## Entities

- Category
- Product
- User
- Order
- OrderDetail
- Payment
- RefreshToken

## Prerequisites

- .NET 8.0 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio or Visual Studio Code

## Setup Instructions

1. **Create the database**:
   - Run the `Database.sql` script to create the database and tables
   - Run the `SeedData.sql` script to populate with sample data

2. **Update the connection string**:
   - Update the connection string in `appsettings.json` if needed

3. **Build the project**:
   ```bash
   dotnet build
   ```

4. **Run the project**:
   ```bash
   dotnet run --project PRN232.Lab2.CoffeeStore.API
   ```

## API Endpoints

### Authentication
- `POST /api/users/login` - Login and get JWT token
- `POST /api/users/refresh-token` - Refresh JWT token
- `POST /api/users/logout` - Logout

### Categories
- `GET /api/categories` - Get all categories with search, sort, paging
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category (Admin only)
- `PUT /api/categories/{id}` - Update category (Admin only)
- `DELETE /api/categories/{id}` - Delete category (Admin only)

### Products
- `GET /api/products` - Get all products with search, sort, paging
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product (Admin only)
- `PUT /api/products/{id}` - Update product (Admin only)
- `DELETE /api/products/{id}` - Delete product (Admin only)

### Users
- `GET /api/users` - Get all users (Admin only)
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user (Admin only)
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user (Admin only)

### Orders
- `GET /api/orders` - Get all orders with search, sort, paging
- `GET /api/orders/{id}` - Get order by ID
- `POST /api/orders` - Create new order (Admin only)
- `PUT /api/orders/{id}` - Update order (Admin only)
- `DELETE /api/orders/{id}` - Delete order (Admin only)

### Order Details
- `GET /api/orderdetails` - Get all order details with search, sort, paging
- `GET /api/orderdetails/{id}` - Get order detail by ID
- `POST /api/orderdetails` - Create new order detail (Admin only)
- `PUT /api/orderdetails/{id}` - Update order detail (Admin only)
- `DELETE /api/orderdetails/{id}` - Delete order detail (Admin only)

### Payments
- `GET /api/payments` - Get all payments with search, sort, paging
- `GET /api/payments/{id}` - Get payment by ID
- `POST /api/payments` - Create new payment (Admin only)
- `PUT /api/payments/{id}` - Update payment (Admin only)
- `DELETE /api/payments/{id}` - Delete payment (Admin only)

## Query Parameters

All GET endpoints support the following query parameters:

- `search` - Search by keyword
- `sortBy` - Sort by field name
- `ascending` - Sort order (true/false)
- `page` - Page number (default: 1)
- `pageSize` - Items per page (default: 10)
- `fields` - Select specific fields to return (comma-separated)

## Authentication

Use the login endpoint to obtain a JWT token:

```
POST /api/users/login
Content-Type: application/json

{
  "email": "admin@coffeestore.com",
  "password": "Admin@123"
}
```

Include the token in the Authorization header for protected endpoints:

```
Authorization: Bearer <your-jwt-token>
```

## Default Users

- Admin: admin@coffeestore.com / Admin@123
- Staff: staff@coffeestore.com / Staff@123

Note: These users are created when you run the SeedData.sql script. The passwords are securely hashed using SHA256 in the database. When you run the seed script, SQL Server automatically hashes the plain text passwords using the HASHBYTES function.

## Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core
- JWT Authentication
- Swagger/OpenAPI
- SQL Server

## License

This project is for educational purposes only.