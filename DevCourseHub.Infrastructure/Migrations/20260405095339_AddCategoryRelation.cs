using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevCourseHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Courses",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql(@"
        INSERT INTO ""Categories"" (""Id"", ""Name"", ""CreatedAt"")
        SELECT gen_random_uuid(), c.""Category"", NOW()
        FROM ""Courses"" c
        WHERE c.""Category"" IS NOT NULL AND c.""Category"" <> ''
        GROUP BY c.""Category"";
    ");

            migrationBuilder.Sql(@"
        UPDATE ""Courses"" c
        SET ""CategoryId"" = cat.""Id""
        FROM ""Categories"" cat
        WHERE c.""Category"" = cat.""Name"";
    ");

            migrationBuilder.Sql(@"
        ALTER TABLE ""Courses""
        ALTER COLUMN ""CategoryId"" SET NOT NULL;
    ");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CategoryId",
                table: "Courses",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Categories_CategoryId",
                table: "Courses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Categories_CategoryId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CategoryId",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Courses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
        UPDATE ""Courses"" c
        SET ""Category"" = cat.""Name""
        FROM ""Categories"" cat
        WHERE c.""CategoryId"" = cat.""Id"";
    ");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
