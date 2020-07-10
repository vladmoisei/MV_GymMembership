using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCWithBlazor.Migrations
{
    public partial class AddModelsGymApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AntrenamentModels",
                columns: table => new
                {
                    AntrenamentModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(nullable: false),
                    OraStart = table.Column<DateTime>(nullable: false),
                    OraStop = table.Column<DateTime>(nullable: true),
                    IsPersonalTraining = table.Column<bool>(nullable: false),
                    Grupa = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntrenamentModels", x => x.AntrenamentModelID);
                });

            migrationBuilder.CreateTable(
                name: "PersAntrAbTables",
                columns: table => new
                {
                    PersAntrAbTableID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersoanaModelID = table.Column<int>(nullable: false),
                    AntrenamentModelID = table.Column<int>(nullable: false),
                    AbonamentModelID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersAntrAbTables", x => x.PersAntrAbTableID);
                });

            migrationBuilder.CreateTable(
                name: "PersoanaModels",
                columns: table => new
                {
                    PersoanaModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(maxLength: 50, nullable: false),
                    Prenume = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Telefon = table.Column<string>(maxLength: 50, nullable: false),
                    DataNastere = table.Column<DateTime>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    IsAcordGDPR = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersoanaModels", x => x.PersoanaModelID);
                });

            migrationBuilder.CreateTable(
                name: "TipAbonamentModels",
                columns: table => new
                {
                    TipAbonamentModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(maxLength: 100, nullable: false),
                    NrTotalSedinte = table.Column<int>(nullable: false),
                    IsPersonalTraining = table.Column<bool>(nullable: false),
                    Pret = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipAbonamentModels", x => x.TipAbonamentModelID);
                });

            migrationBuilder.CreateTable(
                name: "AbonamentModels",
                columns: table => new
                {
                    AbonamentModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStart = table.Column<DateTime>(nullable: false),
                    DataStop = table.Column<DateTime>(nullable: false),
                    StareAbonament = table.Column<int>(nullable: false),
                    NrSedinteEfectuate = table.Column<int>(nullable: false),
                    PersoanaModelID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbonamentModels", x => x.AbonamentModelID);
                    table.ForeignKey(
                        name: "FK_AbonamentModels_PersoanaModels_PersoanaModelID",
                        column: x => x.PersoanaModelID,
                        principalTable: "PersoanaModels",
                        principalColumn: "PersoanaModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbonamentModels_PersoanaModelID",
                table: "AbonamentModels",
                column: "PersoanaModelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbonamentModels");

            migrationBuilder.DropTable(
                name: "AntrenamentModels");

            migrationBuilder.DropTable(
                name: "PersAntrAbTables");

            migrationBuilder.DropTable(
                name: "TipAbonamentModels");

            migrationBuilder.DropTable(
                name: "PersoanaModels");
        }
    }
}
