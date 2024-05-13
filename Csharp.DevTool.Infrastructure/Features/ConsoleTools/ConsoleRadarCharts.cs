namespace Csharp.DevTool.Infrastructure.Features.ConsoleTools;

public class ConsoleRadarCharts
{

    public static void TestChart()
    {
        double[] values = { 4, 3, 5, 2, 3 };
        string[] labels = { "A", "B", "C", "D", "E" };

        // Configure chart parameters
        const int radius = 5;
        const int centerX = 10;
        const int centerY = 10;

        // Draw the radar chart
        DrawRadarChart(values, labels, radius, centerX, centerY);

        // Wait for user input before exiting
        Console.ReadLine();
    }

    static void DrawRadarChart(double[] values, string[] labels, int radius, int centerX, int centerY)
    {
        Console.Clear();

        // Draw axis lines
        Console.SetCursorPosition(centerX, centerY);
        Console.Write("+");
        for (int i = 0; i < values.Length; i++)
        {
            double angle = 2 * Math.PI * i / values.Length;
            int x = (int)Math.Round(centerX + radius * Math.Cos(angle));
            int y = (int)Math.Round(centerY + radius * Math.Sin(angle));
            Console.SetCursorPosition(x, y);
            Console.Write("+");
            Console.SetCursorPosition(centerX, centerY);
        }

        // Draw data points and connecting lines
        for (int i = 0; i < values.Length; i++)
        {
            double angle = 2 * Math.PI * i / values.Length;
            int x = (int)Math.Round(centerX + values[i] * Math.Cos(angle));
            int y = (int)Math.Round(centerY + values[i] * Math.Sin(angle));
            Console.SetCursorPosition(x, y);
            Console.Write("*");
            if (i < values.Length - 1)
            {
                double nextAngle = 2 * Math.PI * (i + 1) / values.Length;
                int nextX = (int)Math.Round(centerX + values[i + 1] * Math.Cos(nextAngle));
                int nextY = (int)Math.Round(centerY + values[i + 1] * Math.Sin(nextAngle));
                DrawLine(x, y, nextX, nextY);
            }
            else
            {
                // Connect the last point to the first point
                double nextAngle = 2 * Math.PI * 0 / values.Length;
                int nextX = (int)Math.Round(centerX + values[0] * Math.Cos(nextAngle));
                int nextY = (int)Math.Round(centerY + values[0] * Math.Sin(nextAngle));
                DrawLine(x, y, nextX, nextY);
            }
        }

        // Draw labels
        for (int i = 0; i < labels.Length; i++)
        {
            double angle = 2 * Math.PI * i / values.Length;
            int x = (int)Math.Round(centerX + (radius + 1) * Math.Cos(angle));
            int y = (int)Math.Round(centerY + (radius + 1) * Math.Sin(angle));
            Console.SetCursorPosition(x, y);
            Console.Write(labels[i]);
        }
    }

    static void DrawLine(int x1, int y1, int x2, int y2)
    {
        int dx = Math.Abs(x2 - x1);
        int dy = Math.Abs(y2 - y1);
        int sx = x1 < x2 ? 1 : -1;
        int sy = y1 < y2 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            Console.SetCursorPosition(x1, y1);
            Console.Write("*");

            if (x1 == x2 && y1 == y2)
                break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y1 += sy;
            }
        }
    }
}
