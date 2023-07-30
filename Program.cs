const int InnerRadius = 8;
const int OuterRadius = 16;
const int QuadrantSize = 25;
const double QuaterInnerRadius = InnerRadius / 4.0;
const double Accuracy = 1E-4;
var pixels = new[] { '.', ':', '-', '=', '+', '*', '#', '%', '@' };

void DrawPixel(char[][] picture, int x, int y, char pixel)
{
    picture[y + QuadrantSize][2 * (x + QuadrantSize)] =
    picture[y + QuadrantSize][2 * (x + QuadrantSize) + 1] =
    pixel;
}

bool IsInBoundRotated(int x, int y, double z, double sinA, double cosA, double sinT, double cosT)
{
    var a = x * cosT + y * sinT;
    var b = x * sinT - y * cosT;
    var c = a * cosA + z * sinA;
    var d = a * sinA - z * cosA;
    var f = Math.Sqrt(b * b + c * c) - OuterRadius;
    return d * d + f * f <= InnerRadius * InnerRadius;
}

double GetZ(int x, int y, double sinA, double cosA, double sinT, double cosT)
{
    var upperBound = OuterRadius + InnerRadius / 2.0;
    var lowerBound = -upperBound;
    var z = upperBound;
    while (z >= lowerBound)
    {
        if (!IsInBoundRotated(x, y, z, sinA, cosA, sinT, cosT))
        {
            z -= QuaterInnerRadius;
            continue;
        }
        while (upperBound - z >= Accuracy)
        {
            var middle = (upperBound + z) / 2;
            if (IsInBoundRotated(x, y, middle, sinA, cosA, sinT, cosT))
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

(double dx, double dy, double dz) GetDerivatives(int x, int y, double z, double sinA, double cosA, double sinT, double cosT)
{
    var a = x * cosT + y * sinT;
    var b = x * sinT - y * cosT;
    var c = a * cosA + z * sinA;
    var d = a * sinA - z * cosA;
    var e = Math.Sqrt(b * b + c * c);
    var f = e - OuterRadius;
    var dx = (f * ((b * sinT + c * cosA * cosT) / e) + d * sinA * cosT);
    var dy = (f * ((c * cosA * sinT - b * cosT) / e) + d * sinA * sinT);
    var dz = (f * c * sinA) / e - d * cosA;
    return (dx, dy, dz);
}

var picture = new char[QuadrantSize * 2][];
for (int i = 0; i < picture.Length; i++)
{
    picture[i] = new char[QuadrantSize * 4];
}

Console.CursorVisible = false;
var alpha = 0.0;
var theta = 0.0;
while (!Console.KeyAvailable)
{
    var sinA = Math.Sin(alpha);
    var cosA = Math.Cos(alpha);
    var sinT = Math.Sin(theta);
    var cosT = Math.Cos(theta);
    for (int y = -QuadrantSize; y < QuadrantSize; y++)
    {
        for (int x = -QuadrantSize; x < QuadrantSize; x++)
        {
            var z = GetZ(x, y, sinA, cosA, sinT, cosT);
            if (double.IsNaN(z))
            {
                DrawPixel(picture, x, y, ' ');
                continue;
            }
            var (dx, dy, dz) = GetDerivatives(x, y, z, sinA, cosA, sinT, cosT);
            var cos = dz / (Math.Sqrt(dx * dx + dy * dy + dz * dz));
            DrawPixel(picture, x, y, pixels[(int)Math.Round(Math.Abs(cos) * 8)]);
        }
        Console.WriteLine(picture[y + QuadrantSize]);
    }
    Thread.Sleep(30);
    Console.Clear();
    alpha += 0.1;
    theta += 0.025;
}
