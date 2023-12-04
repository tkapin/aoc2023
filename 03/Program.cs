namespace aoc03;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var lines = File.ReadAllLines(args[0]);
        var schematic = Schematic.Parse(lines);
        //schematic.Print(Console.Out);
        var pns = schematic.DetectPartNumbers();
        Console.WriteLine("Task 1: " + pns.Sum());
    }
}

internal class Schematic(int width, int height, char[,] schematic)
{
    public int Width { get; } = width;

    public int Height { get; } = height;

    public char[,] Values { get; } = schematic;

    public char Char(int x, int y) => Values[x, y];

    public bool IsDigit(int x, int y) => char.IsDigit(Char(x, y));

    public bool IsSymbol(int x, int y)
    {
        var c = Char(x, y);
        return !(char.IsDigit(c) || c.Equals('.'));
    }

    public bool IsSymbolInSurrounding(int x, int y)
    {
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                if (IsSymbol(x + i, y + j))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Print(TextWriter tw)
    {
        for (int j = 0; j < Height; j++)
        {
            for (int i = 0; i < Width; i++)
            {
                tw.Write(IsSymbolInSurrounding(i + 1, j + 1) ? "x" : ".");
            }
            tw.WriteLine();
        }
    }

    public List<int> DetectPartNumbers()
    {
        List<int> partNumbers = [];

        bool inNumber = false;
        bool pnDetected = false;
        Stack<int> digits = new();
        for (int j = 1; j <= Height; j++)
        {
            for (int i = 1; i <= Width; i++)
            {
                char c = Char(i, j);
                if (char.IsDigit(c))
                {
                    // new number detected
                    if (!inNumber)
                    {
                        digits = new Stack<int>();
                        inNumber = true;
                    }
                    digits.Push(c - '0');
                    pnDetected |= IsSymbolInSurrounding(i, j);
                }
                if (!char.IsDigit(c))
                {
                    if (inNumber)
                    {
                        // convert the digits and zero flags
                        if (pnDetected)
                        {
                            partNumbers.Add(DecodeNumber(digits));
                        }
                        inNumber = false;
                        pnDetected = false;
                    }
                }
            }
        }
        return partNumbers;
    }

    public static int DecodeNumber(Stack<int> stack)
    {
        int i = 0;
        int number = 0;
        while (stack.Count > 0)
        {
            number += (int)Math.Pow(10, i) * stack.Pop();
            i++;
        }
        return number;
    }

    public static Schematic Parse(string[] lines)
    {
        int width = lines[0].Length;
        int height = lines.Length;
        // we'll index the schematic as x,y with x going left and y down
        char[,] schematic = new char[width + 2, height + 2];
        // wrap the schmeatic in a bar of '.' to calculate surroundings
        for (int i = 0; i < width + 2; i++)
        {
            schematic[i, 0] = '.';
            schematic[i, height + 1] = '.';
        }
        for (int j = 0; j < height; j++)
        {
            schematic[0, j + 1] = '.';
            for (int i = 0; i < width; i++)
            {
                schematic[i + 1, j + 1] = lines[j][i];
            }
            schematic[width + 1, j + 1] = '.';
        }
        return new Schematic(width, height, schematic);
    }
}
