using System.Text;

namespace Flyable.Utils;

public class VerifyCodeGenerator
{
    private const int Length = 6;

    private static readonly char[] Chars =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z'
    };

    public string VerifyCode { get; } = GenerateVerifyCode();

    private static string GenerateVerifyCode()
    {
        var random = new Random();
        var sb = new StringBuilder();
        for (var i = 0; i < Length; i++)
        {
            var index = random.Next(Chars.Length);
            sb.Append(Chars[index]);
        }

        return sb.ToString();
    }
}