using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Serilog;

namespace Flyable.Extensions;

public static class QuickOperationExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LogInfo(this string message)
    {
        foreach (var line in message.Split('\n')) Log.Information(line);
    }

}