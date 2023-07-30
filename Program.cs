const int InnerRadius = 8;
const int OuterRadius = 16;
const int QuadrantSize = 25;
const int InnerRadiusSquared = InnerRadius * InnerRadius;
const double HalfInnerRadius = InnerRadius / 2.0;
const double Accuracy = 1E-4;
var pixels = new[] { '.', ':', '-', '=', '+', '*', '#', '%', '@' };

void DrawPixel(char[][] picture, int x, int y, char pixel)
{
    picture[y + QuadrantSize][2 * (x + QuadrantSize)] =
    picture[y + QuadrantSize][2 * (x + QuadrantSize) + 1] =
    pixel;
}

(double dx, double dy, double dz) GetDerivatives(int x, int y, double sinA, double cosA, double sinT, double cosT)
{
    var upperBound = OuterRadius + HalfInnerRadius;
    var lowerBound = -upperBound;
    var z = upperBound;
    while (z >= lowerBound)
    {
        var a = x * cosT + y * sinT;
        var b = x * sinT - y * cosT;
        var c = a * cosA + z * sinA;
        var d = a * sinA - z * cosA;
        var f = Math.Sqrt(b * b + c * c) - OuterRadius;
        if (d * d + f * f > InnerRadiusSquared)
        {
            z -= HalfInnerRadius;
            continue;
        }

        while (upperBound - z >= Accuracy)
        {
            var middle = (upperBound + z) / 2;
            c = a * cosA + middle * sinA;
            d = a * sinA - middle * cosA;
            f = Math.Sqrt(b * b + c * c) - OuterRadius;
            if (d * d + f * f <= InnerRadiusSquared)
            {
                z = middle;
            }
            else
            {
                upperBound = middle;
            }
        }

        var e = f + OuterRadius;
        var dx = (f * ((b * sinT + c * cosA * cosT) / e) + d * sinA * cosT);
        var dy = (f * ((c * cosA * sinT - b * cosT) / e) + d * sinA * sinT);
        var dz = (f * c * sinA) / e - d * cosA;
        return (dx, dy, dz);
    }

    return (double.NaN, double.NaN, double.NaN);
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
            var (dx, dy, dz) = GetDerivatives(x, y, sinA, cosA, sinT, cosT);
            if (double.IsNaN(dz))
            {
                DrawPixel(picture, x, y, ' ');
                continue;
            }
            var cos = dz / (Math.Sqrt(dx * dx + dy * dy + dz * dz));
            DrawPixel(picture, x, y, pixels[(int)Math.Round(cos * 8)]);
        }
        Console.WriteLine(picture[y + QuadrantSize]);
    }
    Thread.Sleep(30);
    Console.Clear();
    alpha += 0.1;
    theta += 0.025;
}
