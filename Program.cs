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

static bool IsInBoundRotated(int x, int y, double angle)
{
    var cos = Math.Cos(angle);
    var sin = Math.Sin(angle);
    var z = (OuterRadius - InnerRadius / 2.0) * sin;
    // why is it == and not != ???
    // crazy!
    if (x >= 0 == cos >= 0)
    {
        z = -z;
    }
    var firstBracket = Math.Sqrt((x * cos - z * sin) * (x * cos - z * sin) + y * y) - OuterRadius;
    var secondBracket = x * sin + z * cos;
    return firstBracket * firstBracket + secondBracket * secondBracket <= InnerRadius * InnerRadius;
}

var angle = 0.0;
while (angle < Math.PI * 2)
{
    for (int x = -QuadrantSize; x < 2 * QuadrantSize; x++)
    {
        for (int y = -QuadrantSize; y < 2 * QuadrantSize; y++)
        {
            if (IsInBoundRotated(x, y, angle))
            {
                DrawPixel(x, y);
            }
        }
    }
    Thread.Sleep(100);
    Console.Clear();
    angle += 0.1;
}
