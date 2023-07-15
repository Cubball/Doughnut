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
    var z = (OuterRadius - InnerRadius / 2.0) * sinXZ;
    var z2 = -z;

    var firstBracket = Math.Sqrt((cosXZ * (x * cosXY + y * sinXY) + z * sinXZ) * (cosXZ * (x * cosXY + y * sinXY) + z * sinXZ) +
            (x * sinXY - y * cosXY) * (x * sinXY - y * cosXY)) - OuterRadius;
    var secondBracket = sinXZ * (x * cosXY + y * sinXY) - z * cosXZ;
    var firstBracket2 = Math.Sqrt((cosXZ * (x * cosXY + y * sinXY) + z2 * sinXZ) * (cosXZ * (x * cosXY + y * sinXY) + z2 * sinXZ) +
            (x * sinXY - y * cosXY) * (x * sinXY - y * cosXY)) - OuterRadius;
    var secondBracket2 = sinXZ * (x * cosXY + y * sinXY) - z2 * cosXZ;
    return firstBracket * firstBracket + secondBracket * secondBracket <= InnerRadius * InnerRadius ||
        firstBracket2 * firstBracket2 + secondBracket2 * secondBracket2 <= InnerRadius * InnerRadius;
}

Console.CursorVisible = false;
var angle = 0.0;
while (!Console.KeyAvailable)
{
    for (int y = -QuadrantSize; y < QuadrantSize; y++)
    {
        for (int x = -QuadrantSize; x < QuadrantSize; x++)
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
