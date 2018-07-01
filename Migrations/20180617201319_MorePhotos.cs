using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnlineStore.Migrations
{
    public partial class MorePhotos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl1",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl2",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrl2",
                table: "Products");
        }
    }
}
