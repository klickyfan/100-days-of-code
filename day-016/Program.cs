using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

/*
https://adventofcode.com/2015/day/16
*/

namespace AdventOfCode
{
    public static class Program
    {
        // https://regex101.com/r/eW80sV/1/
        const string LINE_PATTERN = @"^Sue (?<sue>[\d]+): ([a-z]+): ([\d]+), ([a-z]+): ([\d]+), ([a-z]+): ([\d]+)";

        private static readonly Dictionary<string, int> part1MFCSAM = new Dictionary<string, int>
        {
            { "children", 3},
            { "cats", 7 },
            { "samoyeds", 2 },
            { "pomeranians", 3 },
            { "akitas", 0 },
            { "vizslas", 0 },
            { "goldfish", 5 },
            { "trees", 3 },
            { "cars", 2 },
            { "perfumes", 1 }
        };

        private static readonly Dictionary<string, Func<int, bool>> part2MFCSAM = new Dictionary<string, Func<int, bool>>
        {
            { "children", x => x == 3 },
            { "cats", x => x > 7 },
            { "samoyeds", x => x == 2 },
            { "pomeranians", x => x < 3 },
            { "akitas", x => x == 0 },
            { "vizslas", x => x == 0 },
            { "goldfish", x => x < 5 },
            { "trees", x => x > 3 },
            { "cars", x => x == 2},
            { "perfumes", x => x ==  1 }
        };

        public static void Main()
        {
            Regex regex = new Regex(LINE_PATTERN, RegexOptions.IgnoreCase);

            string input = File.ReadAllText("input.txt");

            Part1(regex, input);
            Part2(regex, input);
        }

        private static void Part1(Regex regex, string input)
        {
            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match match = regex.Match(line);

                    if (match.Success)
                    {
                        var sueNumber = Convert.ToInt32(match.Groups["sue"].Value);

                        if (part1MFCSAM[match.Groups[1].Value] == Convert.ToInt32(match.Groups[2].Value) &&
                            part1MFCSAM[match.Groups[3].Value] == Convert.ToInt32(match.Groups[4].Value) &&
                            part1MFCSAM[match.Groups[5].Value] == Convert.ToInt32(match.Groups[6].Value))
                        {
                            Console.WriteLine($"The correct Sue is #{sueNumber}.");
                        }
                    }
                }
            }
        }

        private static void Part2(Regex regex, string input)
        {
            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match match = regex.Match(line);

                    if (match.Success)
                    {
                        var sueNumber = Convert.ToInt32(match.Groups["sue"].Value);
                        
                        if (part2MFCSAM[match.Groups[1].Value](Convert.ToInt32(match.Groups[2].Value)) &&
                            part2MFCSAM[match.Groups[3].Value](Convert.ToInt32(match.Groups[4].Value)) &&
                            part2MFCSAM[match.Groups[5].Value](Convert.ToInt32(match.Groups[6].Value)))
                        {
                            Console.WriteLine($"The correct Sue is #{sueNumber}.");
                        }
                    }
                }
            }
        }
    }
}
