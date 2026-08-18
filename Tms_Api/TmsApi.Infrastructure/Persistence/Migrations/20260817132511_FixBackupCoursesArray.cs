using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TmsApi.Migrations
{
    /// <inheritdoc />
    public partial class FixBackupCoursesArray : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "BackupCourses",
                table: "Enrollments",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'::text[]");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Enrollments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Term",
                table: "Enrollments",
                type: "text",
                nullable: false,
                defaultValue: "Fall 2026");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackupCourses",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Term",
                table: "Enrollments");
        }
    }
}
