using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnlineStore.Migrations
{
    public partial class PaymentDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreditCardNumber",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreditCardOwner",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpirationDate",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditCardNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreditCardOwner",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Orders");
        }
    }
}
