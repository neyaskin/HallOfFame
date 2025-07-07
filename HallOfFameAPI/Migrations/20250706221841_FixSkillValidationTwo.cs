using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HallOfFameAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixSkillValidationTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Skill_Level",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "Skills",
                newName: "Level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Skills",
                newName: "level");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Skill_Level",
                table: "Skills",
                sql: "Level >= 1 AND Level <= 10");
        }
    }
}
