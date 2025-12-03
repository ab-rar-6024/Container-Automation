using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContainerAutomationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCraneAndShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Workers",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CraneId",
                table: "Workers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Workers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_CraneId",
                table: "Workers",
                column: "CraneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Cranes_CraneId",
                table: "Workers",
                column: "CraneId",
                principalTable: "Cranes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Cranes_CraneId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_CraneId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "CraneId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Workers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
