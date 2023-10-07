using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flyable.Migrations
{
    /// <inheritdoc />
    public partial class mig002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Collections",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Follows",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TagList",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserTags",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(ushort),
                oldType: "smallint unsigned");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTags_UserId",
                table: "UserTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId1",
                table: "Users",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Novels_UserId",
                table: "Novels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_ApplyId",
                table: "Applies",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_UserId",
                table: "Applies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_AdminId",
                table: "Admins",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_Users_UserId",
                table: "Novels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserId1",
                table: "Users",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTags_Users_UserId",
                table: "UserTags",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Novels_Users_UserId",
                table: "Novels");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserId1",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTags_Users_UserId",
                table: "UserTags");

            migrationBuilder.DropIndex(
                name: "IX_UserTags_UserId",
                table: "UserTags");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Novels_UserId",
                table: "Novels");

            migrationBuilder.DropIndex(
                name: "IX_Applies_ApplyId",
                table: "Applies");

            migrationBuilder.DropIndex(
                name: "IX_Applies_UserId",
                table: "Applies");

            migrationBuilder.DropIndex(
                name: "IX_Admins_AdminId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserTags");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Users");

            migrationBuilder.AlterColumn<ushort>(
                name: "Status",
                table: "Users",
                type: "smallint unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Collections",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Follows",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TagList",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
