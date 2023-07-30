const int InnerRadius = 8;
const int OuterRadius = 15;
const int QuadrantSize = 25;
var pixels = new[] { '.', ':', '-', '=', '+', '*', '#', '%', '@', '@' };

void DrawPixel(char[][] picture, int x, int y, char pixel)
{
    picture[y + QuadrantSize][2 * (x + QuadrantSize)] = picture[y + QuadrantSize][2 * (x + QuadrantSize) + 1] = pixel;
}

bool IsInBoundRotated(int x, int y, double z, double sinXZ, double cosXZ, double sinXY, double cosXY)
{
    var firstBracket = Math.Sqrt((cosXZ * (x * cosXY + y * sinXY) + z * sinXZ) * (cosXZ * (x * cosXY + y * sinXY) + z * sinXZ) +
            (x * sinXY - y * cosXY) * (x * sinXY - y * cosXY)) - OuterRadius;
    var secondBracket = sinXZ * (x * cosXY + y * sinXY) - z * cosXZ;
    return firstBracket * firstBracket + secondBracket * secondBracket <= InnerRadius * InnerRadius;
}

double GetZ(int x, int y, double sinXZ, double cosXZ, double sinXY, double cosXY)
{
    var upperBound = OuterRadius + InnerRadius / 2.0;
    var lowerBound = -upperBound;
    var z = upperBound;
    while (z >= lowerBound)
    {
        if (!IsInBoundRotated(x, y, z, sinXZ, cosXZ, sinXY, cosXY))
        {
            z -= InnerRadius / 2.0;
            continue;
        }
        while (upperBound - z >= 1E-4)
        {
            var middle = (upperBound + z) / 2;
            if (IsInBoundRotated(x, y, middle, sinXZ, cosXZ, sinXY, cosXY))
            {
                z = middle;
            }
            else
            {
                upperBound = middle;
            }
        }
        return z;
    }
    return double.NaN;
}

double GetDerivativeWithRespectToX(int x, int y, double z, double sinXZ, double cosXZ, double sinXY, double cosXY)
{
    var root = Math.Sqrt(Math.Pow(cosXZ * (x * cosXY + y * sinXY) + z * sinXZ, 2) + Math.Pow(x * sinXY - y * cosXY, 2));
    return 2 * sinXZ * cosXY * (sinXZ * (x * cosXY + y * sinXY) - z * cosXZ) + (root - OuterRadius) / root *
        (2 * (cosXZ * (x * cosXY + y * sinXY) + z * sinXZ) * cosXZ * cosXY + 2 * sinXY * (x * sinXY - y * cosXY));
}

double GetDerivativeWithRespectToY(int x, int y, double z, double sinXZ, double cosXZ, double sinXY, double cosXY)
{
    var root = Math.Sqrt(Math.Pow(cosXZ * (x * cosXY + y * sinXY) + z * sinXZ, 2) + Math.Pow(x * sinXY - y * cosXY, 2));
    return 2 * sinXZ * cosXY * (sinXZ * (x * cosXY + y * sinXY) - z * cosXZ) + (root - OuterRadius) / root *
        (2 * (cosXZ * (x * cosXY + y * sinXY) + z * sinXZ) * cosXZ * sinXY - 2 * cosXY * (x * sinXY - y * cosXY));
}

double GetDerivativeWithRespectToZ(int x, int y, double z, double sinXZ, double cosXZ, double sinXY, double cosXY)
{
    var root = Math.Sqrt(Math.Pow(cosXZ * (x * cosXY + y * sinXY) + z * sinXZ, 2) + Math.Pow(x * sinXY - y * cosXY, 2));
    return -2 * cosXZ * (sinXZ * (x * cosXY + y * sinXY) - z * cosXZ) +
        (root - OuterRadius) / root * 2 * sinXZ * (cosXZ * (x * cosXY + y * sinXY) + z * sinXZ);
}

var picture = new char[QuadrantSize * 2][];
for (int i = 0; i < picture.Length; i++)
{
    picture[i] = new char[QuadrantSize * 4];
}

Console.CursorVisible = false;
var XZAngle = 0.0;
var XYAngle = 0.0;
while (!Console.KeyAvailable)
{
    var sinXZ = Math.Sin(XZAngle);
    var cosXZ = Math.Cos(XZAngle);
    var sinXY = Math.Sin(XYAngle);
    var cosXY = Math.Cos(XYAngle);
    for (int y = -QuadrantSize; y < QuadrantSize; y++)
    {
        for (int x = -QuadrantSize; x < QuadrantSize; x++)
        {
            var z = GetZ(x, y, sinXZ, cosXZ, sinXY, cosXY);
            if (double.IsNaN(z))
            {
                DrawPixel(picture, x, y, ' ');
                continue;
            }
            var dx = GetDerivativeWithRespectToX(x, y, z, sinXZ, cosXZ, sinXY, cosXY);
            var dy = GetDerivativeWithRespectToY(x, y, z, sinXZ, cosXZ, sinXY, cosXY);
            var dz = GetDerivativeWithRespectToZ(x, y, z, sinXZ, cosXZ, sinXY, cosXY);
            var cos = dz / (Math.Sqrt(dx * dx + dy * dy + dz * dz));
            DrawPixel(picture, x, y, pixels[(int)(Math.Abs(cos) * 9)]);
        }
        Console.WriteLine(picture[y + QuadrantSize]);
    }
    Thread.Sleep(25);
    Console.Clear();
    XZAngle += 0.1;
    XYAngle += 0.025;
}
