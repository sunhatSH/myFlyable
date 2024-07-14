using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flyable.Migrations
{
    /// <inheritdoc />
    public partial class mig10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentTo",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "IsReply",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "CommentNums",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "IsOpenChatRoomMode",
                table: "NovelComments");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "NovelComments");

            migrationBuilder.DropColumn(
                name: "ReportCount",
                table: "NovelComments");

            migrationBuilder.DropColumn(
                name: "CommentCount",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "ChapterComments");

            migrationBuilder.DropColumn(
                name: "ReplyCount",
                table: "ChapterComments");

            migrationBuilder.RenameColumn(
                name: "ReplyCount",
                table: "PostComments",
                newName: "BelongsPostPostId");

            migrationBuilder.AddColumn<int>(
                name: "ChapterCommentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NovelCommentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NovelCommentId1",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostCommentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReplyId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Replies",
                columns: table => new
                {
                    ReplyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReplyTo = table.Column<int>(type: "int", nullable: false),
                    ReplyType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReportCount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ChapterCommentId = table.Column<int>(type: "int", nullable: true),
                    PostCommentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replies", x => x.ReplyId);
                    table.ForeignKey(
                        name: "FK_Replies_ChapterComments_ChapterCommentId",
                        column: x => x.ChapterCommentId,
                        principalTable: "ChapterComments",
                        principalColumn: "ChapterCommentId");
                    table.ForeignKey(
                        name: "FK_Replies_PostComments_PostCommentId",
                        column: x => x.PostCommentId,
                        principalTable: "PostComments",
                        principalColumn: "PostCommentId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChapterCommentId",
                table: "Users",
                column: "ChapterCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NovelCommentId",
                table: "Users",
                column: "NovelCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NovelCommentId1",
                table: "Users",
                column: "NovelCommentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PostCommentId",
                table: "Users",
                column: "PostCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReplyId",
                table: "Users",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_BelongsPostPostId",
                table: "PostComments",
                column: "BelongsPostPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ChapterCommentId",
                table: "Replies",
                column: "ChapterCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_PostCommentId",
                table: "Replies",
                column: "PostCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Posts_BelongsPostPostId",
                table: "PostComments",
                column: "BelongsPostPostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ChapterComments_ChapterCommentId",
                table: "Users",
                column: "ChapterCommentId",
                principalTable: "ChapterComments",
                principalColumn: "ChapterCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_NovelComments_NovelCommentId",
                table: "Users",
                column: "NovelCommentId",
                principalTable: "NovelComments",
                principalColumn: "NovelCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_NovelComments_NovelCommentId1",
                table: "Users",
                column: "NovelCommentId1",
                principalTable: "NovelComments",
                principalColumn: "NovelCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PostComments_PostCommentId",
                table: "Users",
                column: "PostCommentId",
                principalTable: "PostComments",
                principalColumn: "PostCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Replies_ReplyId",
                table: "Users",
                column: "ReplyId",
                principalTable: "Replies",
                principalColumn: "ReplyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Posts_BelongsPostPostId",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ChapterComments_ChapterCommentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_NovelComments_NovelCommentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_NovelComments_NovelCommentId1",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_PostComments_PostCommentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Replies_ReplyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChapterCommentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_NovelCommentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_NovelCommentId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PostCommentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReplyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_BelongsPostPostId",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "ChapterCommentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NovelCommentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NovelCommentId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PostCommentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "BelongsPostPostId",
                table: "PostComments",
                newName: "ReplyCount");

            migrationBuilder.AddColumn<int>(
                name: "CommentTo",
                table: "PostComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsReply",
                table: "PostComments",
                type: "bit(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "PostComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommentNums",
                table: "Novels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpenChatRoomMode",
                table: "NovelComments",
                type: "bit(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "NovelComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReportCount",
                table: "NovelComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommentCount",
                table: "Chapters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "ChapterComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReplyCount",
                table: "ChapterComments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
