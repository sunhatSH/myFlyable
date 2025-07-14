using Flyable.Utils;
using Serilog;
using SkiaSharp;

namespace Flyable.Services;

public class VerifyCodeService
{
    private readonly Random _random = new();
    private const string Chars = "2345678ABCDEFGHJKLMNPQRSTUVWXYZ";

    public (string Code, byte[] ImageBytes) GenerateVerifyCode()
    {
        // 生成4位随机验证码
        var code = new string(Enumerable.Repeat(Chars, 4)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
        Log.Information($"Generated verify code: {code}");

        // 创建图片
        using var surface = SKSurface.Create(new SKImageInfo(120, 40));
        var canvas = surface.Canvas;

        // 设置背景
        canvas.Clear(SKColors.White);

        // 添加干扰线
        for (int i = 0; i < 5; i++)
        {
            var paint = new SKPaint
            {
                Color = GetRandomColor(),
                StrokeWidth = 1,
                IsAntialias = true
            };

            var points = new[]
            {
                new SKPoint(_random.Next(120), _random.Next(40)),
                new SKPoint(_random.Next(120), _random.Next(40))
            };
            canvas.DrawLine(points[0], points[1], paint);
        }

        // 添加干扰点
        for (int i = 0; i < 30; i++)
        {
            var paint = new SKPaint
            {
                Color = GetRandomColor(),
                IsAntialias = true
            };
            canvas.DrawPoint(_random.Next(120), _random.Next(40), paint);
        }

        // 绘制验证码
        var typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);
        var font = new SKFont(typeface, 20);

        // 为每个字符添加随机旋转
        for (var i = 0; i < code.Length; i++)
        {
            var paint = new SKPaint
            {
                Color = GetRandomColor(),
                IsAntialias = true
            };

            var x = 20 + i * 25;
            var y = 25;
            var angle = _random.Next(-30, 30);

            canvas.Save();
            canvas.RotateDegrees(angle, x, y);
            canvas.DrawText(code[i].ToString(), x, y, font, paint);
            canvas.Restore();
        }

        // 转换为字节数组
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return (code, data.ToArray());
    }

    private SKColor GetRandomColor()
    {
        return new SKColor(
            (byte)_random.Next(0, 150),
            (byte)_random.Next(0, 150),
            (byte)_random.Next(0, 150)
        );
    }
}