using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCProject.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRealationBetweenSkillAndTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skills_TeacherId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Communication",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Design",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Development",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Innovation",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "TeamLider",
                table: "Skills",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "Skills",
                newName: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_TeacherId",
                table: "Skills",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skills_TeacherId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Skills",
                newName: "TeamLider");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Skills",
                newName: "Language");

            migrationBuilder.AddColumn<string>(
                name: "Communication",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Design",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Development",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Innovation",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_TeacherId",
                table: "Skills",
                column: "TeacherId",
                unique: true);
        }
    }
}
