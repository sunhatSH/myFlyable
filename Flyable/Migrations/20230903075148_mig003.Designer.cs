﻿// <auto-generated />
using System;
using Flyable.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Flyable.Migrations
{
    [DbContext(typeof(FlyableUserContext))]
    [Migration("20230903075148_mig003")]
    partial class mig003
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Flyable.Repositories.Entities.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Permission")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AdminId");

                    b.HasIndex("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.AdminRecord", b =>
                {
                    b.Property<int>("RecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<string>("AdminName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Detail")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsResigned")
                        .HasColumnType("bit(1)");

                    b.Property<DateTime>("OperateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("OperatorTypeCode")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("longtext");

                    b.HasKey("RecordId");

                    b.ToTable("AdminRecords");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.Apply", b =>
                {
                    b.Property<int>("ApplyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ApplyContent")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ApplyStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("ApplyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ApplyType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ProcessTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ApplyId");

                    b.HasIndex("ApplyId");

                    b.HasIndex("UserId");

                    b.ToTable("Applies");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.Chapter", b =>
                {
                    b.Property<int>("ChapterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AuthorWords")
                        .HasColumnType("longtext");

                    b.Property<int>("BelongsNovelId")
                        .HasColumnType("int");

                    b.Property<string>("ChapterName")
                        .HasColumnType("longtext");

                    b.Property<int>("ChapterOrder")
                        .HasColumnType("int");

                    b.Property<int>("ChapterStatus")
                        .HasColumnType("int");

                    b.Property<int>("CommentCount")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastAlterTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<string>("Shorthand")
                        .HasColumnType("longtext");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.Property<int>("WordCount")
                        .HasColumnType("int");

                    b.HasKey("ChapterId");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.ChapterComment", b =>
                {
                    b.Property<int>("ChapterCommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BelongsChapterChapterId")
                        .HasColumnType("int");

                    b.Property<int>("CommentStatus")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsHot")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsRecommend")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsTop")
                        .HasColumnType("bit(1)");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ReplyCount")
                        .HasColumnType("int");

                    b.Property<int>("ReportCount")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ChapterCommentId");

                    b.HasIndex("BelongsChapterChapterId");

                    b.ToTable("ChapterComments");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.Editor", b =>
                {
                    b.Property<int>("EditorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("EditorName")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("JoinTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("EditorId");

                    b.ToTable("Editors");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.EditorRecord", b =>
                {
                    b.Property<int>("RecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Detail")
                        .HasColumnType("longtext");

                    b.Property<int>("EditorId")
                        .HasColumnType("int");

                    b.Property<string>("EditorName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsResigned")
                        .HasColumnType("bit(1)");

                    b.Property<DateTime>("OperateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("OperateTypeCode")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("longtext");

                    b.HasKey("RecordId");

                    b.ToTable("EditorRecords");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit(1)");

                    b.Property<int>("MessageStatus")
                        .HasColumnType("int");

                    b.Property<int>("MessageTarget")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.HasKey("MessageId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsBroadcast")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit(1)");

                    b.Property<DateTime>("PublishTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.Novel", b =>
                {
                    b.Property<int>("NovelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CollectNums")
                        .HasColumnType("int");

                    b.Property<int>("CommentNums")
                        .HasColumnType("int");

                    b.Property<string>("Cover")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DetailedDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("EditorRecommendReason")
                        .HasColumnType("longtext");

                    b.Property<int>("FavoredNums")
                        .HasColumnType("int");

                    b.Property<bool>("IsAllowComment")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsEditorRecommend")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsOpenChatRoom")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsOriginal")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsShowOnHomePage")
                        .HasColumnType("bit(1)");

                    b.Property<DateTime>("LastAlterTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LastModifyAdminId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<int>("LikeNums")
                        .HasColumnType("int");

                    b.Property<int>("NovelBackground")
                        .HasColumnType("int");

                    b.Property<string>("NovelName")
                        .HasColumnType("longtext");

                    b.Property<int>("NovelOrientation")
                        .HasColumnType("int");

                    b.Property<int>("NovelStatus")
                        .HasColumnType("int");

                    b.Property<string>("NovelTags")
                        .HasColumnType("longtext");

                    b.Property<int>("NovelType")
                        .HasColumnType("int");

                    b.Property<int>("ReportNums")
                        .HasColumnType("int");

                    b.Property<double>("Score")
                        .HasColumnType("double");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("longtext");

                    b.Property<int>("TotalClicks")
                        .HasColumnType("int");

                    b.Property<int>("TotalColorStone")
                        .HasColumnType("int");

                    b.Property<int>("TotalFeather")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WordCount")
                        .HasColumnType("int");

                    b.HasKey("NovelId");

                    b.HasIndex("UserId");

                    b.ToTable("Novels");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.NovelComment", b =>
                {
                    b.Property<int>("NovelCommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BelongsNovelNovelId")
                        .HasColumnType("int");

                    b.Property<int>("CommentStatus")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsHot")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsOpenChatRoomMode")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsRecommend")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsTop")
                        .HasColumnType("bit(1)");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ReplyCount")
                        .HasColumnType("int");

                    b.Property<int>("ReportCount")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NovelCommentId");

                    b.HasIndex("BelongsNovelNovelId");

                    b.ToTable("NovelComments");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsHot")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsRecommend")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsShowOnHomePage")
                        .HasColumnType("bit(1)");

                    b.Property<DateTime>("LastAlterTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LastModifyAdminId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<int>("PostStatus")
                        .HasColumnType("int");

                    b.Property<string>("PostTags")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("PublishTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ReplyCount")
                        .HasColumnType("int");

                    b.Property<int>("ReportCount")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<int>("TotalColorStone")
                        .HasColumnType("int");

                    b.Property<int>("TotalFeather")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("PostId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.PostComment", b =>
                {
                    b.Property<int>("PostCommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BelongsPostPostId")
                        .HasColumnType("int");

                    b.Property<int>("CommentStatus")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsHot")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsRecommend")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsTop")
                        .HasColumnType("bit(1)");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ReplyCount")
                        .HasColumnType("int");

                    b.Property<int>("ReportCount")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PostCommentId");

                    b.HasIndex("BelongsPostPostId");

                    b.ToTable("PostComments");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("ProcessTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("ProcessorId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ReportTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ReportType")
                        .HasColumnType("int");

                    b.Property<int>("ReportedId")
                        .HasColumnType("int");

                    b.Property<int>("ReporterId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ReportId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ColorStone")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<int>("Feather")
                        .HasColumnType("int");

                    b.Property<string>("Followed")
                        .HasColumnType("longtext");

                    b.Property<string>("HeadLink")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsVip")
                        .HasColumnType("bit(1)");

                    b.Property<uint>("LastIp")
                        .HasColumnType("int unsigned");

                    b.Property<DateTime>("LastLoginTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LastModifyAdminId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<ushort>("Level")
                        .HasColumnType("smallint unsigned");

                    b.Property<string>("PwdToken")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ReportCount")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("SelfIntroduction")
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("UserId1")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.Property<int>("WearTagTagId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("UserId1");

                    b.HasIndex("WearTagTagId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.UserTag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastPublishTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TagDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("TagIcon")
                        .HasColumnType("longtext");

                    b.Property<string>("TagName")
                        .HasColumnType("longtext");

                    b.Property<int>("TagRemain")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TagId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTags");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.ChapterComment", b =>
                {
                    b.HasOne("Flyable.Repositories.Entities.Chapter", "BelongsChapter")
                        .WithMany()
                        .HasForeignKey("BelongsChapterChapterId");

                    b.Navigation("BelongsChapter");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.Novel", b =>
                {
                    b.HasOne("Flyable.Repositories.Entities.User", null)
                        .WithMany("Collections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.NovelComment", b =>
                {
                    b.HasOne("Flyable.Repositories.Entities.Novel", "BelongsNovel")
                        .WithMany()
                        .HasForeignKey("BelongsNovelNovelId");

                    b.Navigation("BelongsNovel");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.PostComment", b =>
                {
                    b.HasOne("Flyable.Repositories.Entities.Post", "BelongsPost")
                        .WithMany()
                        .HasForeignKey("BelongsPostPostId");

                    b.Navigation("BelongsPost");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.User", b =>
                {
                    b.HasOne("Flyable.Repositories.Entities.User", null)
                        .WithMany("Follows")
                        .HasForeignKey("UserId1");

                    b.HasOne("Flyable.Repositories.Entities.UserTag", "WearTag")
                        .WithMany()
                        .HasForeignKey("WearTagTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WearTag");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.UserTag", b =>
                {
                    b.HasOne("Flyable.Repositories.Entities.User", null)
                        .WithMany("TagList")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Flyable.Repositories.Entities.User", b =>
                {
                    b.Navigation("Collections");

                    b.Navigation("Follows");

                    b.Navigation("TagList");
                });
#pragma warning restore 612, 618
        }
    }
}
