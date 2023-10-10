using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VebTechTEstApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("2de178b4-17e0-40f5-b438-86c0affbc061"), new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(6073), new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(6074), false, "Support" },
                    { new Guid("3cdeabd8-fc68-4c53-8b02-27321f1e67c3"), new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(6068), new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(6069), false, "SuperAdmin" },
                    { new Guid("3fe309fb-5d09-4f0a-ad05-2efe76280fe2"), new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(6077), new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(6077), false, "User" },
                    { new Guid("c4668177-fca6-4894-b8e6-ff926c7c0e49"), new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(5996), new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(6003), false, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "DateCreated", "DateUpdated", "Email", "IsDeleted", "Name", "Password" },
                values: new object[] { new Guid("44b2d7af-bf9f-4a51-9eae-3dfe153ef315"), 31, new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(6084), new DateTime(2023, 10, 10, 12, 14, 41, 515, DateTimeKind.Utc).AddTicks(6084), "superadmin@gmail.com", false, "SuperAdmin", "25D55AD283AA400AF464C76D713C07AD" });

            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesId", "UsersId" },
                values: new object[] { new Guid("3cdeabd8-fc68-4c53-8b02-27321f1e67c3"), new Guid("44b2d7af-bf9f-4a51-9eae-3dfe153ef315") });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
