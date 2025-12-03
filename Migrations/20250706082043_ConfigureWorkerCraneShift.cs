using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContainerAutomationApp.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureWorkerCraneShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Cranes_AssignedCraneId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Cranes_CraneId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Shifts_AssignedShiftId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Shifts_ShiftId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_CraneId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_ShiftId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "CraneId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Workers");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Cranes_AssignedCraneId",
                table: "Workers",
                column: "AssignedCraneId",
                principalTable: "Cranes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Shifts_AssignedShiftId",
                table: "Workers",
                column: "AssignedShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Cranes_AssignedCraneId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Shifts_AssignedShiftId",
                table: "Workers");

            migrationBuilder.AddColumn<int>(
                name: "CraneId",
                table: "Workers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "Workers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_CraneId",
                table: "Workers",
                column: "CraneId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_ShiftId",
                table: "Workers",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Cranes_AssignedCraneId",
                table: "Workers",
                column: "AssignedCraneId",
                principalTable: "Cranes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Cranes_CraneId",
                table: "Workers",
                column: "CraneId",
                principalTable: "Cranes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Shifts_AssignedShiftId",
                table: "Workers",
                column: "AssignedShiftId",
                principalTable: "Shifts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Shifts_ShiftId",
                table: "Workers",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id");
        }
    }
}
