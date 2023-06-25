const int QuadrantSize = 20;
const int InnerRadius = 5;
const int OuterRadius = 10;
const double RotationAngle = Math.PI / 9;

static void DrawPixel(int x, int y)
{
    Console.SetCursorPosition(2 * (x + QuadrantSize), y + QuadrantSize);
    Console.Write("##");
}

static bool IsInBound(int x, int y)
{
    var foo = Math.Sqrt(x * x + y * y) - OuterRadius;
    return foo * foo <= InnerRadius * InnerRadius;
}

static bool IsInBoundRotated(int x, int y)
{
    var cos = Math.Cos(RotationAngle);
    var sin = Math.Sin(RotationAngle);
    var foo = Math.Sqrt(x * x + y * y * cos * cos) - OuterRadius;
    return foo * foo + y * y * sin * sin <= InnerRadius * InnerRadius;
}

for (int x = -QuadrantSize; x < 2 * QuadrantSize; x++)
{
    for (int y = -QuadrantSize; y < 2 * QuadrantSize; y++)
    {
        if (IsInBoundRotated(x, y))
        {
            DrawPixel(x, y);
        }
    }
}

Console.ReadKey();
