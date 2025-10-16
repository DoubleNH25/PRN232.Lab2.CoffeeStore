-- Create database
CREATE DATABASE CoffeeStoreDB;
GO

USE CoffeeStoreDB;
GO

-- Create Category table
CREATE TABLE Category (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    CreatedDate DATETIME2 DEFAULT GETDATE()
);
GO

-- Create Product table
CREATE TABLE Product (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(18,2) NOT NULL,
    CategoryId INT FOREIGN KEY REFERENCES Category(CategoryId),
    IsActive BIT DEFAULT 1
);
GO

-- Create User table (since we're not using AppUser/Identity)
CREATE TABLE [User] (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Staff')),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);
GO

-- Create Order table
CREATE TABLE [Order] (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES [User](UserId),
    OrderDate DATETIME2 DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Pending',
    TotalAmount DECIMAL(18,2) DEFAULT 0
);
GO

-- Create Payment table
CREATE TABLE Payment (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES [Order](OrderId),
    Amount DECIMAL(18,2) NOT NULL,
    PaymentDate DATETIME2 DEFAULT GETDATE(),
    PaymentMethod NVARCHAR(50)
);
GO

-- Create OrderDetail table
CREATE TABLE OrderDetail (
    OrderDetailId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES [Order](OrderId),
    ProductId INT FOREIGN KEY REFERENCES Product(ProductId),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL
);
GO

-- Create RefreshToken table for JWT refresh tokens
CREATE TABLE RefreshToken (
    RefreshTokenId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES [User](UserId),
    Token NVARCHAR(500) NOT NULL,
    Expires DATETIME2 NOT NULL,
    Created DATETIME2 DEFAULT GETDATE(),
    Revoked DATETIME2 NULL
);
GO