using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillMatrix.service.Migrations
{
    /// <inheritdoc />
    public partial class createDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkillsMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SkillDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SkillStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeSkills",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersId = table.Column<int>(type: "int", nullable: false),
                    SkillsMasterId = table.Column<int>(type: "int", nullable: false),
                    SkillType = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    SkillRating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeSkills", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeSkills_SkillsMaster_SkillsMasterId",
                        column: x => x.SkillsMasterId,
                        principalTable: "SkillsMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeSkills_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SkillsMaster",
                columns: new[] { "Id", "SkillDescription", "SkillName", "SkillStatus" },
                values: new object[,]
                {
                    { 1, "Angular is a frond end language", "ANGULAR", 1 },
                    { 2, "DotNet is a back end language", "DOTNET", 1 },
                    { 3, "React is a frond end language", "REACT", 1 },
                    { 4, "Node is a back end language", "NODE", 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "admin@gmail.com", "Admin", "YWRtaW5AMTIzNDU=", null },
                    { 2, "employe1@gmail.com", "employe1", "MTIzNDU2Nzg=", null },
                    { 3, "employe2@gmail.com", "employe2", "MTIzNDU2Nzg=", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeSkills_SkillsMasterId",
                table: "EmployeSkills",
                column: "SkillsMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeSkills_UsersId",
                table: "EmployeSkills",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeSkills");

            migrationBuilder.DropTable(
                name: "SkillsMaster");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
