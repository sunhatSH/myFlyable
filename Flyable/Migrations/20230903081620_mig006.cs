using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flyable.Migrations
{
    /// <inheritdoc />
    public partial class mig006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostTags",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "NovelTags",
                table: "Novels");

            migrationBuilder.RenameColumn(
                name: "BelongsNovelId",
                table: "Chapters",
                newName: "NovelId");

            migrationBuilder.AddColumn<int>(
                name: "ScorePeopleNum",
                table: "Novels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NovelTag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TagName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TagDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelTag", x => x.TagId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TagName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TagDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => x.TagId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NovelNovelTag",
                columns: table => new
                {
                    NovelTagsTagId = table.Column<int>(type: "int", nullable: false),
                    NovelsNovelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelNovelTag", x => new { x.NovelTagsTagId, x.NovelsNovelId });
                    table.ForeignKey(
                        name: "FK_NovelNovelTag_NovelTag_NovelTagsTagId",
                        column: x => x.NovelTagsTagId,
                        principalTable: "NovelTag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NovelNovelTag_Novels_NovelsNovelId",
                        column: x => x.NovelsNovelId,
                        principalTable: "Novels",
                        principalColumn: "NovelId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PostPostTag",
                columns: table => new
                {
                    PostTagsTagId = table.Column<int>(type: "int", nullable: false),
                    PostsPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostPostTag", x => new { x.PostTagsTagId, x.PostsPostId });
                    table.ForeignKey(
                        name: "FK_PostPostTag_PostTag_PostTagsTagId",
                        column: x => x.PostTagsTagId,
                        principalTable: "PostTag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostPostTag_Posts_PostsPostId",
                        column: x => x.PostsPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NovelNovelTag_NovelsNovelId",
                table: "NovelNovelTag",
                column: "NovelsNovelId");

            migrationBuilder.CreateIndex(
                name: "IX_PostPostTag_PostsPostId",
                table: "PostPostTag",
                column: "PostsPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NovelNovelTag");

            migrationBuilder.DropTable(
                name: "PostPostTag");

            migrationBuilder.DropTable(
                name: "NovelTag");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropColumn(
                name: "ScorePeopleNum",
                table: "Novels");

            migrationBuilder.RenameColumn(
                name: "NovelId",
                table: "Chapters",
                newName: "BelongsNovelId");

            migrationBuilder.AddColumn<string>(
                name: "PostTags",
                table: "Posts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NovelTags",
                table: "Novels",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
