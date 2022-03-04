using Microsoft.EntityFrameworkCore.Migrations;

namespace hub.dbMigration.Migrations
{
    public partial class faxtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FaxNumbers",
                columns: table => new
                {
                    FaxId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaxName = table.Column<string>(type: "varchar(50)", nullable: true),
                    Number = table.Column<string>(type: "varchar(50)", nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaxNumbers", x => x.FaxId);
                    table.ForeignKey(
                        name: "FK_FaxNumbers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaxNumbers_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FaxNumbers_DepartmentId",
                table: "FaxNumbers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FaxNumbers_LocationId",
                table: "FaxNumbers",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaxNumbers");
        }
    }
}
