using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexteAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSkinAndCloakUrlsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "uuid",
                table: "Users",
                type: "char(36)",
                nullable: true, 
                defaultValue: Guid.NewGuid(),
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AddColumn<string>(
                name: "cloakUrl",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "skinUrl",
                table: "Users",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cloakUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "skinUrl",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "uuid",
                table: "Users",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);
        }
    }
}
