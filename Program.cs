const int QuadrantSize = 20;
const int InnerRadius = 5;
const int OuterRadius = 10;

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

for (int x = -QuadrantSize; x < 2 * QuadrantSize; x++)
{
    for (int y = -QuadrantSize; y < 2 * QuadrantSize; y++)
    {
        if (IsInBound(x, y))
        {
            DrawPixel(x, y);
        }
    }
}

Console.ReadKey();
