using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode;

public class Program
{
    public static void Main(string[] args)
    {
        // 1. If command-line arguments are provided, allow running a specific day:
        //    e.g. "10" or "2015 10"
        int? targetYear = null;
        int? targetDay = null;

        if (args.Length == 1 && int.TryParse(args[0], out var day))
        {
            targetDay = day;
        }
        else if (args.Length >= 2 && int.TryParse(args[0], out var y) && int.TryParse(args[1], out var d))
        {
            targetYear = y;
            targetDay = d;
        }

        // 2. Discover all solver classes inheriting from BaseSolver
        var solverTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BaseSolver)) && !t.IsAbstract)
            .Select(t =>
            {
                try
                {
                    var instance = (BaseSolver)Activator.CreateInstance(t)!;
                    return (Type: t, Solver: instance);
                }
                catch
                {
                    return (Type: t, Solver: (BaseSolver)null!);
                }
            })
            .Where(x => x.Solver != null)
            .ToList();

        BaseSolver? solverToRun = null;

        if (targetDay.HasValue)
        {
            solverToRun = solverTypes
                .Where(x => (!targetYear.HasValue || x.Solver.Year == targetYear.Value) && x.Solver.Day == targetDay.Value)
                .OrderByDescending(x => x.Solver.Year)
                .Select(x => x.Solver)
                .FirstOrDefault();

            if (solverToRun == null)
            {
                Console.WriteLine($"Solver for {(targetYear.HasValue ? $"Year {targetYear} " : "")}Day {targetDay} not found.");
                return;
            }
        }
        else
        {
            // Pick the solver file that was most recently edited on disk so that
            // whichever file you are currently writing code in runs automatically!
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            while (baseDir != null && !Directory.Exists(Path.Combine(baseDir, "Inputs")))
            {
                baseDir = Directory.GetParent(baseDir)?.FullName;
            }
            if (baseDir == null) baseDir = Directory.GetCurrentDirectory();

            var latestModifiedFile = Directory.GetFiles(baseDir, "Day*.cs", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTimeUtc)
                .FirstOrDefault();

            if (latestModifiedFile != null)
            {
                var typeName = Path.GetFileNameWithoutExtension(latestModifiedFile.Name);
                solverToRun = solverTypes
                    .Where(x => x.Type.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(x => latestModifiedFile.FullName.Contains(x.Solver.Year.ToString()) ? 1 : 0)
                    .Select(x => x.Solver)
                    .FirstOrDefault();
            }

            // Fallback if file matching fails
            solverToRun ??= solverTypes
                .OrderByDescending(x => x.Solver.Year * 100 + x.Solver.Day)
                .Select(x => x.Solver)
                .FirstOrDefault();
        }

        if (solverToRun != null)
        {
            solverToRun.Solve();
        }
        else
        {
            Console.WriteLine("No solvers found in the project.");
        }
    }
}
