using System.Text;

namespace Flyable.Utils;

public static class VerifyCodeGenerator
{
    private const int Length = 4;

    private static readonly char[] Chars =
    [

        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
    ];

public static ReadOnlySpan<char> VerifyCode
    {
        get
        {
            var random = new Random();
            Span<char> sb = stackalloc char[Length];
            for (var i = 0; i < Length; i++)
            {
                var index = random.Next(Chars.Length);
                sb[i] = Chars[index];
            }
            return sb.ToArray();
        }
    }
}