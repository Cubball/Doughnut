const int InnerRadius = 8;
const int OuterRadius = 16;
const int QuadrantSize = 25;
const double QuaterInnerRadius = InnerRadius / 4.0;
const double Accuracy = 1E-4;
var pixels = new[] { '.', ':', '-', '=', '+', '*', '#', '%', '@' };

void DrawPixel(char[][] picture, int x, int y, char pixel)
{
    picture[y + QuadrantSize][2 * (x + QuadrantSize)]
        = picture[y + QuadrantSize][2 * (x + QuadrantSize) + 1]
        = pixel;
}

bool IsInBoundRotated(int x, int y, double z, double sinXZ, double cosXZ, double sinXY, double cosXY)
{
    var a = x * cosXY + y * sinXY;
    var b = x * sinXY - y * cosXY;
    var c = a * cosXZ + z * sinXZ;
    var d = a * sinXZ - z * cosXZ;
    var f = Math.Sqrt(b * b + c * c) - OuterRadius;
    return d * d + f * f <= InnerRadius * InnerRadius;
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
            z -= QuaterInnerRadius;
            continue;
        }
        while (upperBound - z >= Accuracy)
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

(double dx, double dy, double dz) GetDerivatives(int x, int y, double z, double sinXZ, double cosXZ, double sinXY, double cosXY)
{
    var a = x * cosXY + y * sinXY;
    var b = x * sinXY - y * cosXY;
    var c = a * cosXZ + z * sinXZ;
    var d = a * sinXZ - z * cosXZ;
    var e = Math.Sqrt(b * b + c * c);
    var f = e - OuterRadius;
    var dx = (f * ((b * sinXY + c * cosXZ * cosXY) / e) + d * sinXZ * cosXY);
    var dy = (f * ((c * sinXY * cosXZ - b * cosXY) / e) + d * sinXZ * sinXY);
    var dz = ((f * c * sinXZ) / e - d * cosXZ);
    return (dx, dy, dz);
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
            var (dx, dy, dz) = GetDerivatives(x, y, z, sinXZ, cosXZ, sinXY, cosXY);
            var cos = dz / (Math.Sqrt(dx * dx + dy * dy + dz * dz));
            DrawPixel(picture, x, y, pixels[(int)Math.Round(Math.Abs(cos) * 8)]);
        }
        Console.WriteLine(picture[y + QuadrantSize]);
    }
    Thread.Sleep(30);
    Console.Clear();
    XZAngle += 0.1;
    XYAngle -= 0.025;
}
