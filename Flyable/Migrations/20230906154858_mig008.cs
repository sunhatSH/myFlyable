using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flyable.Migrations
{
    /// <inheritdoc />
    public partial class mig008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Posts_BelongsPostPostId",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_BelongsPostPostId",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "BelongsPostPostId",
                table: "PostComments");

            migrationBuilder.AddColumn<bool>(
                name: "IsReply",
                table: "PostComments",
                type: "bit(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CommentTo",
                table: "PostComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostId_UserId_LastAlterTime_PublishTime",
                table: "Posts",
                columns: new[] { "PostId", "UserId", "LastAlterTime", "PublishTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostId_UserId_LastAlterTime_PublishTime",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsReply",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "CommentTo",
                table: "PostComments");

            migrationBuilder.AddColumn<int>(
                name: "ReportType",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BelongsPostPostId",
                table: "PostComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_BelongsPostPostId",
                table: "PostComments",
                column: "BelongsPostPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Posts_BelongsPostPostId",
                table: "PostComments",
                column: "BelongsPostPostId",
                principalTable: "Posts",
                principalColumn: "PostId");
        }
    }
}