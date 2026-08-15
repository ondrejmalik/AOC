using System;
using System.IO;
using NUnit.Framework;

namespace AdventOfCode;

public abstract class BaseSolver : ISolver
{
    public abstract int Year { get; }
    public abstract int Day { get; }

    [Test]
    public void Solve()
    {
        Console.WriteLine($"--- Year {Year} Day {Day} ---");
        
        string[] input;
        try
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            while (baseDir != null && !Directory.Exists(Path.Combine(baseDir, "Inputs")))
            {
                baseDir = Directory.GetParent(baseDir)?.FullName;
            }

            if (baseDir == null)
            {
                // Fallback to current working directory
                baseDir = Directory.GetCurrentDirectory();
            }

            var filePath = Path.Combine(baseDir, "Inputs", Year.ToString(), $"day{Day:D2}.txt");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Input file not found at: {filePath}");
                Console.WriteLine("Please create the file and paste your puzzle input.");
                return;
            }
            input = File.ReadAllLines(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading input file: {ex.Message}");
            return;
        }

        Console.WriteLine("Part 1: " + SolvePart1(input));
        Console.WriteLine("Part 2: " + SolvePart2(input));
        Console.WriteLine();
    }

    public abstract string SolvePart1(string[] input);
    public abstract string SolvePart2(string[] input);
}
