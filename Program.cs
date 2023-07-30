int size = Math.Min(Console.WindowWidth, Console.WindowHeight) / 2 - 3;
int r = (size - 1) / 3;
int R = 2 * r;
int rSquared = r * r;
double rHalf = r / 2.0;
const double Accuracy = 1E-4;

var pixels = new[] { '.', ':', '-', '=', '+', '*', '#', '%', '@' };

void DrawPixel(char[][] picture, int x, int y, char pixel)
{
    picture[y + size][2 * (x + size)] =
    picture[y + size][2 * (x + size) + 1] =
    pixel;
}

var picture = new char[size * 2][];
for (int i = 0; i < picture.Length; i++)
{
    picture[i] = new char[size * 4];
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
    for (int y = -size; y < size; y++)
    {
        for (int x = -size; x < size; x++)
        {
            var skip = false;
            var upperBound = R + rHalf;
            var lowerBound = -upperBound;
            var z = upperBound;
            var a = x * cosT + y * sinT;
            var b = x * sinT - y * cosT;
            while (z >= lowerBound)
            {
                var c = a * cosA + z * sinA;
                var d = a * sinA - z * cosA;
                var f = Math.Sqrt(b * b + c * c) - R;
                if (d * d + f * f > rSquared)
                {
                    z -= rHalf;
                    continue;
                }

                while (upperBound - z >= Accuracy)
                {
                    var middle = (upperBound + z) / 2;
                    c = a * cosA + middle * sinA;
                    d = a * sinA - middle * cosA;
                    f = Math.Sqrt(b * b + c * c) - R;
                    if (d * d + f * f <= rSquared)
                    {
                        z = middle;
                    }
                    else
                    {
                        upperBound = middle;
                    }
                }

                var e = f + R;
                var dx = (f * ((b * sinT + c * cosA * cosT) / e) + d * sinA * cosT);
                var dy = (f * ((c * cosA * sinT - b * cosT) / e) + d * sinA * sinT);
                var dz = (f * c * sinA) / e - d * cosA;
                var cos = dz / (Math.Sqrt(dx * dx + dy * dy + dz * dz));
                DrawPixel(picture, x, y, pixels[(int)Math.Round(cos * 8)]);
                skip = true;
                break;
            }

            if (!skip)
            {
                DrawPixel(picture, x, y, ' ');
            }
        }
        Console.WriteLine(picture[y + size]);
    }
    Thread.Sleep(30);
    Console.Clear();
    alpha += 0.1;
    theta += 0.025;
}
