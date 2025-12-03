using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContainerAutomationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkerNavigationToShiftAndCrane : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "Workers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Shifts",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_ShiftId",
                table: "Workers",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Shifts_ShiftId",
                table: "Workers",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Shifts_ShiftId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_ShiftId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Workers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Shifts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
