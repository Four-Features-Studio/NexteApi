using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace NexteAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "longtext", nullable: true),
                    password = table.Column<string>(type: "longtext", nullable: true),
                    avatar = table.Column<string>(type: "longtext", nullable: true),
                    uuid = table.Column<Guid>(type: "char(36)", nullable: true),
                    accessToken = table.Column<string>(type: "longtext", nullable: true),
                    serverId = table.Column<string>(type: "longtext", nullable: true),
                    onlineChat = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    multiplayerServer = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    multiplayerRealms = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    telemetry = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
