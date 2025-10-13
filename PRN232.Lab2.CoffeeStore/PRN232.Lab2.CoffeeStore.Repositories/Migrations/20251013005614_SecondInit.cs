using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232.Lab2.CoffeeStore.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SecondInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "A1F6E36D-38F5-4C7D-8A0A-4F6DDF70F9C5", null, "Administrator", "ADMINISTRATOR" },
                    { "F8C1E04F-4E55-4389-AE23-5AF74B4BEE73", null, "Staff", "STAFF" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "08d9dbc7-9580-42db-bf12-25ac2cd98277", "AQAAAAIAAYagAAAAEDL5BVUHAtas8Z1iQ8Iuk+iOFieAW7MxzcohZ5K4sd8Mtkq1N0eXF1g0ndspTrmqEw==", "a851d0d7-f3f7-4916-ac85-4822aa13c87a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c250c2a8-7045-47c7-80f8-59a6861b7b92", "AQAAAAIAAYagAAAAEJQQHxNm6upe+n5guG1inSG5T4rOVWouqsO2pC+OUoGl3npISRqmgo4jyEntqGbqLQ==", "5ea4dd55-7650-405f-9638-9340393198db" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "A1F6E36D-38F5-4C7D-8A0A-4F6DDF70F9C5", "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" },
                    { "F8C1E04F-4E55-4389-AE23-5AF74B4BEE73", "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "A1F6E36D-38F5-4C7D-8A0A-4F6DDF70F9C5", "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "F8C1E04F-4E55-4389-AE23-5AF74B4BEE73", "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "A1F6E36D-38F5-4C7D-8A0A-4F6DDF70F9C5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "F8C1E04F-4E55-4389-AE23-5AF74B4BEE73");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a7c33e7-6eb5-4557-9b4f-b7168f5b6b3d", "AQAAAAIAAYagAAAAENZAV7E3Y8OERJBfmFY4jWRjVDtFS8oZykIEeSc6ilWY5Sg8L9UqIIF3miKPovtITA==", "e60a909b-7610-4860-8f59-b3ec9e349b0f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6b0ef17a-6a04-48cc-ae05-9f6bd55e6cb0", "AQAAAAIAAYagAAAAEKeTrcUWGxz00fpnml2OFU1Z3IJ5l/48eXatUfIxBQADKteDuqpqTMlcI2X6eGQ39g==", "bd4fe0a8-4e81-4689-8990-9efaeeaaa7c6" });
        }
    }
}
