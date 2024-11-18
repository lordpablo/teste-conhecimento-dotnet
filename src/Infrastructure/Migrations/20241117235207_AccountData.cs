using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleTest.Infrastructure.Migrations
{
    public partial class AccountData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "Balance", "ClientId", "CreateUserId", "CreatedAt", "DeleteUserId", "DeletedAt", "IsDeleted", "Overdraft", "UpdateUserId", "UpdatedAt" },
                values: new object[] { 1, 0.0, 1, null, new DateTime(2024, 11, 15, 19, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 3000.0, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
