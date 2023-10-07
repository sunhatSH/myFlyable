using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flyable.Migrations
{
    /// <inheritdoc />
    public partial class mig007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NovelNovelTag_NovelTag_NovelTagsTagId",
                table: "NovelNovelTag");

            migrationBuilder.DropForeignKey(
                name: "FK_PostPostTag_PostTag_PostTagsTagId",
                table: "PostPostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NovelTag",
                table: "NovelTag");

            migrationBuilder.RenameTable(
                name: "PostTag",
                newName: "PostTags");

            migrationBuilder.RenameTable(
                name: "NovelTag",
                newName: "NovelTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags",
                column: "TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NovelTags",
                table: "NovelTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_NovelNovelTag_NovelTags_NovelTagsTagId",
                table: "NovelNovelTag",
                column: "NovelTagsTagId",
                principalTable: "NovelTags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostPostTag_PostTags_PostTagsTagId",
                table: "PostPostTag",
                column: "PostTagsTagId",
                principalTable: "PostTags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NovelNovelTag_NovelTags_NovelTagsTagId",
                table: "NovelNovelTag");

            migrationBuilder.DropForeignKey(
                name: "FK_PostPostTag_PostTags_PostTagsTagId",
                table: "PostPostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NovelTags",
                table: "NovelTags");

            migrationBuilder.RenameTable(
                name: "PostTags",
                newName: "PostTag");

            migrationBuilder.RenameTable(
                name: "NovelTags",
                newName: "NovelTag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag",
                column: "TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NovelTag",
                table: "NovelTag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_NovelNovelTag_NovelTag_NovelTagsTagId",
                table: "NovelNovelTag",
                column: "NovelTagsTagId",
                principalTable: "NovelTag",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostPostTag_PostTag_PostTagsTagId",
                table: "PostPostTag",
                column: "PostTagsTagId",
                principalTable: "PostTag",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
