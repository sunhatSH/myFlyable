using Flyable.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flyable.Repositories;

public class FlyableUserContext : DbContext
{
    public FlyableUserContext(DbContextOptions<FlyableUserContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Editor> Editors { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<AdminRecord> AdminRecords { get; set; } = null!;
    public DbSet<EditorRecord> EditorRecords { get; set; } = null!;
    public DbSet<Novel> Novels { get; set; } = null!;
    public DbSet<Chapter>? Chapters { get; set; }
    public DbSet<ChapterComment>? ChapterComments { get; set; }
    public DbSet<NovelComment>? NovelComments { get; set; }
    public DbSet<Post>? Posts { get; set; }
    public DbSet<PostComment>? PostComments { get; set; }
    public DbSet<UserTag> UserTags { get; set; } = null!;
    public DbSet<Notification>? Notifications { get; set; }
    public DbSet<Apply>? Applies { get; set; }
    public DbSet<Report>? Reports { get; set; }
    public DbSet<NovelTag>? NovelTags { get; set; }
    public DbSet<PostTag>? PostTags { get; set; }
    public DbSet<Reply>? Replies { get; set; }
    ~FlyableUserContext() //在析构函数中销毁对象
    {
        SaveChangesAsync();
        Dispose();
    }
}