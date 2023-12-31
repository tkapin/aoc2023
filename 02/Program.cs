﻿namespace aoc02;

internal class Program
{
    static void Main(string[] args)
    {
        var bag = Set.Parse("12 red, 13 green, 14 blue");

        List<Game> games = [];
        var lines = File.ReadAllLines(args[0]);
        foreach (var line in lines)
        {
            games.Add(Game.Parse(line));
        }
        Console.WriteLine("Task 1: " + games.Where(x => x.IsPossible(bag)).Sum(x => x.Id));
        Console.WriteLine("Task 2: " + games.Sum(x => x.Union.Power));
    }
}

internal class Set
{
    const string RED = "red";
    const string GREEN = "green";
    const string BLUE = "blue";

    private static readonly string[] s_colors = [RED, GREEN, BLUE];

    public readonly Dictionary<string, int> Cubes = [];

    public Set()
    {
        foreach (var color in s_colors)
        {
            Cubes[color] = 0;
        }
    }

    // Example: "3 blue, 4 red"
    public static Set Parse(string s)
    {
        var set = new Set();
        var parts = s.Split(',');
        foreach (var part in parts)
        {
            var countAndColor = part.Trim().Split(" ");
            set.Cubes[countAndColor[1]] = int.Parse(countAndColor[0]);
        }
        return set;
    }

    public static Set Union(Set[] sets)
    {
        var result = new Set();
        foreach (var color in s_colors)
        {
            result.Cubes[color] = sets.Max(x => x.Cubes[color]);
        }
        return result;
    }

    public bool IsSubsetOf(Set set)
    {
        foreach (var color in s_colors)
        {
            if (Cubes[color] > set.Cubes[color])
            {
                return false;
            }
        }
        return true;
    }

    // conrete colors used as that's how the power is defined
    public int Power => Cubes[RED] * Cubes[GREEN] * Cubes[BLUE];
    
    public override string ToString() => string.Join(", ", s_colors.Select(x => $"{Cubes[x]} {x}"));
}

internal class Game(int id, Set[] sets)
{
    public int Id { get; } = id;

    public Set[] Sets { get; } = sets;

    public Set Union => Set.Union(Sets);

    public bool IsPossible(Set bag) => Sets.All(x => x.IsSubsetOf(bag));

    // Example: "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
    public static Game Parse(string s)
    {
        var gameAndSets = s.Split(':');
        int id = int.Parse(gameAndSets[0].Split(" ")[1]);

        List<Set> sets = [];
        foreach (string setInput in gameAndSets[1].Split(";"))
        {
            sets.Add(Set.Parse(setInput));
        }
        return new Game(id, [.. sets]);
    }

    public override string ToString() => $"Game {Id}: {string.Join("; ", Sets.Select(x => x.ToString()))}";
}
