using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContainerAutomationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveRequest",
                table: "Workers");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Workers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AssignmentHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    CraneId = table.Column<int>(type: "int", nullable: true),
                    ShiftId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentHistories_Cranes_CraneId",
                        column: x => x.CraneId,
                        principalTable: "Cranes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssignmentHistories_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssignmentHistories_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CraneAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CraneId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraneAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CraneAssignments_Cranes_CraneId",
                        column: x => x.CraneId,
                        principalTable: "Cranes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CraneAssignments_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkerAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    CraneAssignmentId = table.Column<int>(type: "int", nullable: false),
                    WorkDuration = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    RestDuration = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    PunchInTime = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PunchOutTime = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerAssignments_CraneAssignments_CraneAssignmentId",
                        column: x => x.CraneAssignmentId,
                        principalTable: "CraneAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerAssignments_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHistories_CraneId",
                table: "AssignmentHistories",
                column: "CraneId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHistories_ShiftId",
                table: "AssignmentHistories",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHistories_WorkerId",
                table: "AssignmentHistories",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_CraneAssignments_CraneId",
                table: "CraneAssignments",
                column: "CraneId");

            migrationBuilder.CreateIndex(
                name: "IX_CraneAssignments_ShiftId",
                table: "CraneAssignments",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerAssignments_CraneAssignmentId",
                table: "WorkerAssignments",
                column: "CraneAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerAssignments_WorkerId",
                table: "WorkerAssignments",
                column: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentHistories");

            migrationBuilder.DropTable(
                name: "WorkerAssignments");

            migrationBuilder.DropTable(
                name: "CraneAssignments");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Workers");

            migrationBuilder.AddColumn<bool>(
                name: "LeaveRequest",
                table: "Workers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
