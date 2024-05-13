using Spectre.Console;
using System.Drawing;

public static class Print
{
    private static readonly ConsoleColor originalForegroundColor = Console.ForegroundColor;
    private static readonly ConsoleColor originalBackgroundColor = Console.BackgroundColor;

    public static void SetColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor = default)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
    }

    public static void ResetColors()
    {
        Console.ForegroundColor = originalForegroundColor;
        Console.BackgroundColor = originalBackgroundColor;
    }

    public static void Error(string message)
    {
        SetColors(ConsoleColor.Red);
        Console.WriteLine(message);
        ResetColors();
    }

    public static void Ascii(string message)
    {
        int R = 225;
        int G = 255;
        int B = 250;

        Colorful.Console.WriteAscii(message);

    }

    public static void Code(string message)
    {
        AnsiConsole.Markup(message);
    }

    public static void Warning(string message)
    {
        SetColors(ConsoleColor.Yellow);
        Console.WriteLine(message);
        ResetColors();
    }


    public static void Info(string message)
    {
        SetColors(ConsoleColor.Blue);
        Console.WriteLine(message);
        ResetColors();
    }


    public static void Normal(string message)
    {
        SetColors(ConsoleColor.White);
        Console.WriteLine(message);
        ResetColors();
    }

}
