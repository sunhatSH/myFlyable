using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;

namespace Flyable.Repositories.DataAccess.DataBaseAccess.Access;

public class EditorAccess : IEditorAccess
{
    private static string _a = "a";

    public static string A
    {
        get => _a;
        set => _a = value ?? throw new ArgumentNullException(nameof(value));
    }
}