using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleTest.Infrastructure.Migrations
{
    public partial class fixAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Client_ClientId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ClientId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Agency",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Agency", "CreatedAt" },
                values: new object[] { "0001", new DateTime(2024, 11, 15, 22, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthDate", "CreatedAt" },
                values: new object[] { new DateTime(1990, 3, 3, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 15, 22, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 2, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agency",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 15, 19, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthDate", "CreatedAt" },
                values: new object[] { new DateTime(1990, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 15, 19, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ClientId", "CreatedAt" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_User_ClientId",
                table: "User",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Client_ClientId",
                table: "User",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
