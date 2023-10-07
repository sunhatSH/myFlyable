using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Services.IServices;

namespace Flyable.Services.Services;

public class EditorService : IEditorService
{
    public EditorService(IEditorAccess editorAccess)
    {
        EditorAccess = editorAccess;
    }

    public IEditorAccess EditorAccess { get; set; }

    public async Task<int> RecommendNovel(string novelId)
    {
        return 0;
    }

    public async Task<int> RecommendPost(string postId)
    {
        return 0;
    }

    public async Task<int> LockNovel(string novelId)
    {
        return 0;
    }

    public async Task<int> LockPost(string postId)
    {
        return 0;
    }

    public async Task<int> LockComment(string commentId)
    {
        return 0;
    }

    public async Task<int> LockChapter(string chapterId)
    {
        return 0;
    }

    public async Task<int> RecommendEditor(string userId)
    {
        return 0;
    }

    public async Task<int> Resign(string userId)
    {
        return 0;
    }

    public async Task<int> PublishNotification(string content, int targetId)
    {
        return 0;
    }

    public async Task<int> WithdrawNotification(int notificationId)
    {
        return 0;
    }

    public async Task<int> SendMessage(string content, int targetId)
    {
        return 0;
    }

    public async Task<int> WithdrawMessage(int messageId)
    {
        return 0;
    }
}