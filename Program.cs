const int QuadrantSize = 20;
const int InnerRadius = 5;
const int OuterRadius = 10;

static void DrawPixel(int x, int y)
{
    Console.SetCursorPosition(2 * (x + QuadrantSize), y + QuadrantSize);
    Console.Write("##");
}

static bool IsInBoundRotated(int x, int y, double XZAngle, double XYAngle)
{
    var sinXZ = Math.Sin(XZAngle);
    var cosXZ = Math.Cos(XZAngle);
    var sinXY = Math.Sin(XYAngle);
    var cosXY = Math.Cos(XYAngle);
    // TODO: figure out how to check for z's sign and swap it if necessary
    var z = (OuterRadius - InnerRadius / 2.0) * sinXZ;

    var firstBracket = Math.Sqrt(((x * cosXY - y * sinXY) * cosXZ - z * sinXZ) *
            ((x * cosXY - y * sinXY) * cosXZ - z * sinXZ) + (x * sinXY + y * cosXY) * (x * sinXY + y * cosXY)) - OuterRadius;
    var secondBracket = (x * cosXY - y * sinXY) * sinXZ + z * cosXZ;
    return firstBracket * firstBracket + secondBracket * secondBracket <= InnerRadius * InnerRadius;
}

Console.CursorVisible = false;
var angle = 0.0;
while (!Console.KeyAvailable)
{
    for (int x = -QuadrantSize; x < 2 * QuadrantSize; x++)
    {
        for (int y = -QuadrantSize; y < 2 * QuadrantSize; y++)
        {
            if (IsInBoundRotated(x, y, angle, angle))
            {
                DrawPixel(x, y);
            }
        }
    }
    Thread.Sleep(100);
    Console.Clear();
    angle += 0.1;
}
