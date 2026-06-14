using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project1.Migrations
{
    /// <inheritdoc />
    public partial class FitnessManagementSystemDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "salons",
                columns: table => new
                {
                    SalonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalonAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalonAdres = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salons", x => x.SalonID);
                });

            migrationBuilder.CreateTable(
                name: "supplements",
                columns: table => new
                {
                    SupplementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplementAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplementFiyati = table.Column<int>(type: "int", nullable: false),
                    SalonID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplements", x => x.SupplementID);
                    table.ForeignKey(
                        name: "FK_supplements_salons_SalonID",
                        column: x => x.SalonID,
                        principalTable: "salons",
                        principalColumn: "SalonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "uyes",
                columns: table => new
                {
                    UyeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UyeAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UyeSoyadi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Yas = table.Column<int>(type: "int", nullable: false),
                    SalonID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uyes", x => x.UyeID);
                    table.ForeignKey(
                        name: "FK_uyes_salons_SalonID",
                        column: x => x.SalonID,
                        principalTable: "salons",
                        principalColumn: "SalonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "antrenors",
                columns: table => new
                {
                    AntrenorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Yas = table.Column<int>(type: "int", nullable: false),
                    TecrübeYili = table.Column<int>(type: "int", nullable: false),
                    UyeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_antrenors", x => x.AntrenorID);
                    table.ForeignKey(
                        name: "FK_antrenors_uyes_UyeID",
                        column: x => x.UyeID,
                        principalTable: "uyes",
                        principalColumn: "UyeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_antrenors_UyeID",
                table: "antrenors",
                column: "UyeID");

            migrationBuilder.CreateIndex(
                name: "IX_supplements_SalonID",
                table: "supplements",
                column: "SalonID");

            migrationBuilder.CreateIndex(
                name: "IX_uyes_SalonID",
                table: "uyes",
                column: "SalonID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "antrenors");

            migrationBuilder.DropTable(
                name: "supplements");

            migrationBuilder.DropTable(
                name: "uyes");

            migrationBuilder.DropTable(
                name: "salons");
        }
    }
}
