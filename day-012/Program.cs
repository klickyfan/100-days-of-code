using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

// https://adventofcode.com/2015/day/12

public class Program
{
    public static void Main()
    {
        string input = File.ReadAllText("input.json");

        Part1(input);

        Part2(input);
    }

    private static void Part1(string input)
    {
        string pattern = @"(-?\d+)";
        RegexOptions options = RegexOptions.Multiline;

        long sum = 0;
        foreach (Match m in Regex.Matches(input, pattern, options))
        {
            sum += Convert.ToInt32(m.Value);
        }

        Console.WriteLine($"Part 1 sum: {sum}");
    }

    private static void Part2(string input)
    {
        Console.WriteLine($"Part 2 sum: {Sum(JObject.Parse(input))}");
    }

    private static long Sum(JToken token)
    {
        switch (token)
        {
            case JObject o:
                if (o.Properties()
                        .Select(p => p.Value).OfType<JValue>()
                        .Select(v => v.Value).OfType<string>()
                        .Any(s => s == "red"))
                {
                    return 0;
                }
                return o.Properties().Select(p => p.Value).Sum(t => Sum(t));
            case JArray a:
                return a.Sum(jt => Sum(jt));
            case JValue v:
                return (v.Value is long) ? (long)v.Value : 0;
        }

        throw new Exception();
    }
}
