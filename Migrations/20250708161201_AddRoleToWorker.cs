using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContainerAutomationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToWorker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentHistories_Cranes_CraneId",
                table: "AssignmentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentHistories_Shifts_ShiftId",
                table: "AssignmentHistories");

            migrationBuilder.AlterColumn<int>(
                name: "ShiftId",
                table: "AssignmentHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CraneId",
                table: "AssignmentHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AssignmentHistories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AssignmentHistories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentHistories_Cranes_CraneId",
                table: "AssignmentHistories",
                column: "CraneId",
                principalTable: "Cranes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentHistories_Shifts_ShiftId",
                table: "AssignmentHistories",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentHistories_Cranes_CraneId",
                table: "AssignmentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentHistories_Shifts_ShiftId",
                table: "AssignmentHistories");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AssignmentHistories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AssignmentHistories");

            migrationBuilder.AlterColumn<int>(
                name: "ShiftId",
                table: "AssignmentHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CraneId",
                table: "AssignmentHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentHistories_Cranes_CraneId",
                table: "AssignmentHistories",
                column: "CraneId",
                principalTable: "Cranes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentHistories_Shifts_ShiftId",
                table: "AssignmentHistories",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id");
        }
    }
}
