using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Serilog;

namespace Flyable.Extensions;

public static class QuickOperationExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Print<T>(this T t)
    {
        Console.WriteLine(t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LogInfo(this string message)
    {
        foreach (var line in message.Split('\n')) Log.Information(line);
    }

    public static string ToJson<T>(this T t)
    {
        return JsonConvert.SerializeObject(t);
    }
}