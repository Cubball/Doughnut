const int InnerRadius = 5;
const int OuterRadius = 10;
const int QuadrantSize = 20;

void DrawPixel(int x, int y)
{
    Console.SetCursorPosition(2 * (x + QuadrantSize), y + QuadrantSize);
    Console.Write("@@");
}

bool IsInBoundRotated(int x, int y, double XZAngle, double XYAngle)
{
    var sinXZ = Math.Sin(XZAngle);
    var cosXZ = Math.Cos(XZAngle);
    var sinXY = Math.Sin(XYAngle);
    var cosXY = Math.Cos(XYAngle);
    var z = (OuterRadius - InnerRadius / 2.0) * sinXZ;
    if (cosXZ > 0 != x * cosXY + y * sinXY > 0)
    {
        z = -z;
    }
    var firstBracket = Math.Sqrt((cosXZ * (x * cosXY + y * sinXY) + z * sinXZ) * (cosXZ * (x * cosXY + y * sinXY) + z * sinXZ) +
            (x * sinXY - y * cosXY) * (x * sinXY - y * cosXY)) - OuterRadius;
    var secondBracket = sinXZ * (x * cosXY + y * sinXY) - z * cosXZ;
    return firstBracket * firstBracket + secondBracket * secondBracket <= InnerRadius * InnerRadius;
}

Console.CursorVisible = false;
var angle = 0.0;
while (!Console.KeyAvailable)
{
    for (int y = -QuadrantSize; y <= QuadrantSize; y++)
    {
        for (int x = -QuadrantSize; x <= QuadrantSize; x++)
        {
            if (IsInBoundRotated(x, y, angle, angle / 2))
            {
                DrawPixel(x, y);
            }
        }
    }
    Thread.Sleep(50);
    Console.Clear();
    angle += 0.1;
}
