using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCWithBlazor.Migrations
{
    public partial class AddTipAbToAbonament : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipAbonamentModelID",
                table: "AbonamentModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AbonamentModels_TipAbonamentModelID",
                table: "AbonamentModels",
                column: "TipAbonamentModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_AbonamentModels_TipAbonamentModels_TipAbonamentModelID",
                table: "AbonamentModels",
                column: "TipAbonamentModelID",
                principalTable: "TipAbonamentModels",
                principalColumn: "TipAbonamentModelID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbonamentModels_TipAbonamentModels_TipAbonamentModelID",
                table: "AbonamentModels");

            migrationBuilder.DropIndex(
                name: "IX_AbonamentModels_TipAbonamentModelID",
                table: "AbonamentModels");

            migrationBuilder.DropColumn(
                name: "TipAbonamentModelID",
                table: "AbonamentModels");
        }
    }
}
