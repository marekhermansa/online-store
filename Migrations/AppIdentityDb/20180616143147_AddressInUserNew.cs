using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnlineStore.Migrations.AppIdentityDb
{
    public partial class AddressInUserNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qualifications",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Line1",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Line2",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Line1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Line2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "City",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Qualifications",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
