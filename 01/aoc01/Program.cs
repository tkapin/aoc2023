namespace aoc01;

internal class Program
{
    static void Main(string[] args)
    {
        int sum = 0;
        var lines = File.ReadAllLines(args[0]);
        foreach (var line in lines)
        {
            var digits = GetDigits(line);
            int number = 10 * digits[0] + digits[^1];
            Console.WriteLine($"'{line}' -> [{string.Join(", ", digits)}] -> {number}");
            sum += number;
        }
        Console.WriteLine(sum);
    }

    static int[] GetDigits(string s)
    {
        string[] digits = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
        
        int index = 0;
        List<int> result = [];
        while(index < s.Length)
        {
            char c = s[index];
            if (char.IsDigit(c))
            {
                result.Add(c - '0');
            }
            for (int i = 0; i < digits.Length; i++)
            {
                if (s.IndexOf(digits[i], index) == index)
                {
                    result.Add(i + 1);
                }
            }
            index++;
        }
        return [.. result];
    }
}
