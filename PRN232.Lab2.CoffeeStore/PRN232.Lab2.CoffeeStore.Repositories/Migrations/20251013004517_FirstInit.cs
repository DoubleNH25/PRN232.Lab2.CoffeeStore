using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232.Lab2.CoffeeStore.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class FirstInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    RefreshTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.RefreshTokenId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedDate", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedDate", "UserName" },
                values: new object[,]
                {
                    { "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722", 0, "8a7c33e7-6eb5-4557-9b4f-b7168f5b6b3d", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@coffeestore.com", true, "Coffee Store Admin", false, null, "ADMIN@COFFEESTORE.COM", "ADMIN@COFFEESTORE.COM", "AQAAAAIAAYagAAAAENZAV7E3Y8OERJBfmFY4jWRjVDtFS8oZykIEeSc6ilWY5Sg8L9UqIIF3miKPovtITA==", null, false, "e60a909b-7610-4860-8f59-b3ec9e349b0f", false, null, "admin@coffeestore.com" },
                    { "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD", 0, "6b0ef17a-6a04-48cc-ae05-9f6bd55e6cb0", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "staff@coffeestore.com", true, "Coffee Store Staff", false, null, "STAFF@COFFEESTORE.COM", "STAFF@COFFEESTORE.COM", "AQAAAAIAAYagAAAAEKeTrcUWGxz00fpnml2OFU1Z3IJ5l/48eXatUfIxBQADKteDuqpqTMlcI2X6eGQ39g==", null, false, "bd4fe0a8-4e81-4689-8990-9efaeeaaa7c6", false, null, "staff@coffeestore.com" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CreatedDate", "Description", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 1 description", "Category 1", null },
                    { 2, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 2 description", "Category 2", null },
                    { 3, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 3 description", "Category 3", null },
                    { 4, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 4 description", "Category 4", null },
                    { 5, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 5 description", "Category 5", null },
                    { 6, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 6 description", "Category 6", null },
                    { 7, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 7 description", "Category 7", null },
                    { 8, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 8 description", "Category 8", null },
                    { 9, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 9 description", "Category 9", null },
                    { 10, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 10 description", "Category 10", null },
                    { 11, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 11 description", "Category 11", null },
                    { 12, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 12 description", "Category 12", null },
                    { 13, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 13 description", "Category 13", null },
                    { 14, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 14 description", "Category 14", null },
                    { 15, new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 15 description", "Category 15", null },
                    { 16, new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 16 description", "Category 16", null },
                    { 17, new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 17 description", "Category 17", null },
                    { 18, new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 18 description", "Category 18", null },
                    { 19, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 19 description", "Category 19", null },
                    { 20, new DateTime(2024, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category 20 description", "Category 20", null }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "OrderId", "CreatedDate", "OrderDate", "PaymentId", "Status", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Processing", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 2, new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Pending", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 3, new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Shipped", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 4, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Completed", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 5, new DateTime(2024, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Processing", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 6, new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Pending", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 7, new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Shipped", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 8, new DateTime(2024, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "Completed", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 9, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Processing", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 10, new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Pending", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 11, new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "Shipped", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 12, new DateTime(2024, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "Completed", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 13, new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "Processing", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 14, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, "Pending", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 15, new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "Shipped", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 16, new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, "Completed", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 17, new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, "Processing", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 18, new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, "Pending", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 19, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, "Shipped", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 20, new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, "Completed", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" }
                });

            migrationBuilder.InsertData(
                table: "RefreshToken",
                columns: new[] { "RefreshTokenId", "CreatedDate", "ExpiryDate", "IsRevoked", "Token", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-1", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 2, new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-2", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 3, new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-3", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 4, new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-4", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 5, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-5", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 6, new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-6", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 7, new DateTime(2024, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-7", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 8, new DateTime(2024, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-8", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 9, new DateTime(2024, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-9", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 10, new DateTime(2024, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-10", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 11, new DateTime(2024, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-11", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 12, new DateTime(2024, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-12", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 13, new DateTime(2024, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-13", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 14, new DateTime(2024, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-14", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 15, new DateTime(2024, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-15", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 16, new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-16", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 17, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-17", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 18, new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-18", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { 19, new DateTime(2024, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-19", null, "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" },
                    { 20, new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "seed-refresh-token-20", null, "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "PaymentId", "Amount", "CreatedDate", "OrderId", "PaymentDate", "PaymentMethod", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 30m, new DateTime(2024, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 2, 35m, new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null },
                    { 3, 40m, new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 4, 45m, new DateTime(2024, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2024, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null },
                    { 5, 50m, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 6, 55m, new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null },
                    { 7, 60m, new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 8, 65m, new DateTime(2024, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, new DateTime(2024, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null },
                    { 9, 70m, new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 10, 75m, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null },
                    { 11, 80m, new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 12, 85m, new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null },
                    { 13, 90m, new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 14, 95m, new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null },
                    { 15, 100m, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 16, 105m, new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null },
                    { 17, 110m, new DateTime(2024, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, new DateTime(2024, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 18, 115m, new DateTime(2024, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, new DateTime(2024, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null },
                    { 19, 120m, new DateTime(2024, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, new DateTime(2024, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 20, 125m, new DateTime(2024, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, new DateTime(2024, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", null }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductId", "CategoryId", "CreatedDate", "Description", "IsActive", "Name", "Price", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 1 description", true, "Product 1", 4.5m, null },
                    { 2, 1, new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 2 description", true, "Product 2", 5.5m, null },
                    { 3, 1, new DateTime(2024, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 3 description", true, "Product 3", 6.5m, null },
                    { 4, 1, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 4 description", true, "Product 4", 7.5m, null },
                    { 5, 2, new DateTime(2024, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 5 description", true, "Product 5", 8.5m, null },
                    { 6, 2, new DateTime(2024, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 6 description", true, "Product 6", 9.5m, null },
                    { 7, 2, new DateTime(2024, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 7 description", true, "Product 7", 10.5m, null },
                    { 8, 2, new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 8 description", true, "Product 8", 11.5m, null },
                    { 9, 3, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 9 description", true, "Product 9", 12.5m, null },
                    { 10, 3, new DateTime(2024, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 10 description", true, "Product 10", 13.5m, null },
                    { 11, 3, new DateTime(2024, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 11 description", true, "Product 11", 14.5m, null },
                    { 12, 3, new DateTime(2024, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 12 description", true, "Product 12", 15.5m, null },
                    { 13, 4, new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 13 description", true, "Product 13", 16.5m, null },
                    { 14, 4, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 14 description", true, "Product 14", 17.5m, null },
                    { 15, 4, new DateTime(2024, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 15 description", true, "Product 15", 18.5m, null },
                    { 16, 4, new DateTime(2024, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 16 description", true, "Product 16", 19.5m, null },
                    { 17, 5, new DateTime(2024, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 17 description", true, "Product 17", 20.5m, null },
                    { 18, 5, new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 18 description", true, "Product 18", 21.5m, null },
                    { 19, 5, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 19 description", true, "Product 19", 22.5m, null },
                    { 20, 5, new DateTime(2024, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 20 description", true, "Product 20", 23.5m, null },
                    { 21, 6, new DateTime(2024, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 21 description", true, "Product 21", 24.5m, null },
                    { 22, 6, new DateTime(2024, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 22 description", true, "Product 22", 25.5m, null },
                    { 23, 6, new DateTime(2024, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 23 description", true, "Product 23", 26.5m, null },
                    { 24, 6, new DateTime(2024, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 24 description", true, "Product 24", 27.5m, null },
                    { 25, 7, new DateTime(2024, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 25 description", true, "Product 25", 28.5m, null },
                    { 26, 7, new DateTime(2024, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 26 description", true, "Product 26", 29.5m, null },
                    { 27, 7, new DateTime(2024, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 27 description", true, "Product 27", 30.5m, null },
                    { 28, 7, new DateTime(2024, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 28 description", true, "Product 28", 31.5m, null },
                    { 29, 8, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 29 description", true, "Product 29", 32.5m, null },
                    { 30, 8, new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 30 description", true, "Product 30", 33.5m, null },
                    { 31, 8, new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 31 description", true, "Product 31", 34.5m, null },
                    { 32, 8, new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 32 description", true, "Product 32", 35.5m, null },
                    { 33, 9, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 33 description", true, "Product 33", 36.5m, null },
                    { 34, 9, new DateTime(2024, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 34 description", true, "Product 34", 37.5m, null },
                    { 35, 9, new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 35 description", true, "Product 35", 38.5m, null },
                    { 36, 9, new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 36 description", true, "Product 36", 39.5m, null },
                    { 37, 10, new DateTime(2024, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 37 description", true, "Product 37", 40.5m, null },
                    { 38, 10, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 38 description", true, "Product 38", 41.5m, null },
                    { 39, 10, new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 39 description", true, "Product 39", 42.5m, null },
                    { 40, 10, new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 40 description", true, "Product 40", 43.5m, null },
                    { 41, 11, new DateTime(2024, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 41 description", true, "Product 41", 44.5m, null },
                    { 42, 11, new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 42 description", true, "Product 42", 45.5m, null },
                    { 43, 11, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 43 description", true, "Product 43", 46.5m, null },
                    { 44, 11, new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 44 description", true, "Product 44", 47.5m, null },
                    { 45, 12, new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 45 description", true, "Product 45", 48.5m, null },
                    { 46, 12, new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 46 description", true, "Product 46", 49.5m, null },
                    { 47, 12, new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 47 description", true, "Product 47", 50.5m, null },
                    { 48, 12, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 48 description", true, "Product 48", 51.5m, null },
                    { 49, 13, new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 49 description", true, "Product 49", 52.5m, null },
                    { 50, 13, new DateTime(2024, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 50 description", true, "Product 50", 53.5m, null },
                    { 51, 13, new DateTime(2024, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 51 description", true, "Product 51", 54.5m, null },
                    { 52, 13, new DateTime(2024, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 52 description", true, "Product 52", 55.5m, null },
                    { 53, 14, new DateTime(2024, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 53 description", true, "Product 53", 56.5m, null },
                    { 54, 14, new DateTime(2024, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 54 description", true, "Product 54", 57.5m, null },
                    { 55, 14, new DateTime(2024, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 55 description", true, "Product 55", 58.5m, null },
                    { 56, 14, new DateTime(2024, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 56 description", true, "Product 56", 59.5m, null },
                    { 57, 15, new DateTime(2024, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 57 description", true, "Product 57", 60.5m, null },
                    { 58, 15, new DateTime(2024, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 58 description", true, "Product 58", 61.5m, null },
                    { 59, 15, new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 59 description", true, "Product 59", 62.5m, null },
                    { 60, 15, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 60 description", true, "Product 60", 63.5m, null },
                    { 61, 16, new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 61 description", true, "Product 61", 64.5m, null },
                    { 62, 16, new DateTime(2024, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 62 description", true, "Product 62", 65.5m, null },
                    { 63, 16, new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 63 description", true, "Product 63", 66.5m, null },
                    { 64, 16, new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 64 description", true, "Product 64", 67.5m, null },
                    { 65, 17, new DateTime(2024, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 65 description", true, "Product 65", 68.5m, null },
                    { 66, 17, new DateTime(2024, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 66 description", true, "Product 66", 69.5m, null },
                    { 67, 17, new DateTime(2024, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 67 description", true, "Product 67", 70.5m, null },
                    { 68, 17, new DateTime(2024, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 68 description", true, "Product 68", 71.5m, null },
                    { 69, 18, new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 69 description", true, "Product 69", 72.5m, null },
                    { 70, 18, new DateTime(2024, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 70 description", true, "Product 70", 73.5m, null },
                    { 71, 18, new DateTime(2024, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 71 description", true, "Product 71", 74.5m, null },
                    { 72, 18, new DateTime(2024, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 72 description", true, "Product 72", 75.5m, null },
                    { 73, 19, new DateTime(2024, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 73 description", true, "Product 73", 76.5m, null },
                    { 74, 19, new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 74 description", true, "Product 74", 77.5m, null },
                    { 75, 19, new DateTime(2024, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 75 description", true, "Product 75", 78.5m, null },
                    { 76, 19, new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 76 description", true, "Product 76", 79.5m, null },
                    { 77, 20, new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 77 description", true, "Product 77", 80.5m, null },
                    { 78, 20, new DateTime(2024, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 78 description", true, "Product 78", 81.5m, null },
                    { 79, 20, new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 79 description", true, "Product 79", 82.5m, null },
                    { 80, 20, new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 80 description", true, "Product 80", 83.5m, null }
                });

            migrationBuilder.InsertData(
                table: "OrderDetail",
                columns: new[] { "OrderDetailId", "CreatedDate", "OrderId", "ProductId", "Quantity", "UnitPrice", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 2, 4.5m, null },
                    { 2, new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 3, 5.5m, null },
                    { 3, new DateTime(2024, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 5, 3, 8.5m, null },
                    { 4, new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 6, 1, 9.5m, null },
                    { 5, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 9, 1, 12.5m, null },
                    { 6, new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 10, 2, 13.5m, null },
                    { 7, new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 13, 2, 16.5m, null },
                    { 8, new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14, 3, 17.5m, null },
                    { 9, new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 17, 3, 20.5m, null },
                    { 10, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 18, 1, 21.5m, null },
                    { 11, new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 21, 1, 24.5m, null },
                    { 12, new DateTime(2024, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 22, 2, 25.5m, null },
                    { 13, new DateTime(2024, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 25, 2, 28.5m, null },
                    { 14, new DateTime(2024, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 26, 3, 29.5m, null },
                    { 15, new DateTime(2024, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 29, 3, 32.5m, null },
                    { 16, new DateTime(2024, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 30, 1, 33.5m, null },
                    { 17, new DateTime(2024, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 33, 1, 36.5m, null },
                    { 18, new DateTime(2024, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 34, 2, 37.5m, null },
                    { 19, new DateTime(2024, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 37, 2, 40.5m, null },
                    { 20, new DateTime(2024, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 38, 3, 41.5m, null },
                    { 21, new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 41, 3, 44.5m, null },
                    { 22, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 42, 1, 45.5m, null },
                    { 23, new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 45, 1, 48.5m, null },
                    { 24, new DateTime(2024, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 46, 2, 49.5m, null },
                    { 25, new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 49, 2, 52.5m, null },
                    { 26, new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 50, 3, 53.5m, null },
                    { 27, new DateTime(2024, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 53, 3, 56.5m, null },
                    { 28, new DateTime(2024, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 54, 1, 57.5m, null },
                    { 29, new DateTime(2024, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 57, 1, 60.5m, null },
                    { 30, new DateTime(2024, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 58, 2, 61.5m, null },
                    { 31, new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 61, 2, 64.5m, null },
                    { 32, new DateTime(2024, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 62, 3, 65.5m, null },
                    { 33, new DateTime(2024, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 65, 3, 68.5m, null },
                    { 34, new DateTime(2024, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 66, 1, 69.5m, null },
                    { 35, new DateTime(2024, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, 69, 1, 72.5m, null },
                    { 36, new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, 70, 2, 73.5m, null },
                    { 37, new DateTime(2024, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 73, 2, 76.5m, null },
                    { 38, new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 74, 3, 77.5m, null },
                    { 39, new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 77, 3, 80.5m, null },
                    { 40, new DateTime(2024, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 78, 1, 81.5m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                table: "Payment",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
