using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var groundTruthPath = args.Length > 0
    ? Path.GetFullPath(args[0])
    : Path.GetFullPath("ground-truth.json");

var repoRoot = Path.GetDirectoryName(groundTruthPath)!;
var assemblyPath = args.Length > 1
    ? Path.GetFullPath(args[1])
    : Path.Combine(repoRoot, "AdventOfCode", "bin", "Release", "net11.0", "AdventOfCode.dll");

if (!File.Exists(groundTruthPath))
{
    Console.Error.WriteLine($"Ground truth file not found: {groundTruthPath}");
    return 1;
}

if (!File.Exists(assemblyPath))
{
    Console.Error.WriteLine($"Target assembly not found: {assemblyPath}");
    return 1;
}

var inputsRoot = Path.Combine(repoRoot, "AdventOfCode", "Inputs");
if (!Directory.Exists(inputsRoot))
{
    Console.Error.WriteLine($"Inputs directory not found: {inputsRoot}");
    return 1;
}

var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var config = JsonSerializer.Deserialize<GroundTruthConfig>(
    File.ReadAllText(groundTruthPath),
    jsonOptions) ?? throw new InvalidOperationException("Failed to parse ground-truth.json");

var excluded = new HashSet<string>(
    config.ExcludeSolvers ?? [],
    StringComparer.Ordinal);

var loadContext = new PluginLoadContext(assemblyPath);
var aocAssembly = loadContext.LoadFromAssemblyPath(assemblyPath);

var baseSolverType = aocAssembly.GetType("AdventOfCode.BaseSolver")
    ?? throw new InvalidOperationException("Could not find AdventOfCode.BaseSolver in assembly.");
var yearProp = baseSolverType.GetProperty("Year")!;
var dayProp = baseSolverType.GetProperty("Day")!;
var part1Method = baseSolverType.GetMethod("SolvePart1", [typeof(string[])])!;
var part2Method = baseSolverType.GetMethod("SolvePart2", [typeof(string[])])!;
var stopProgressMethod = aocAssembly.GetType("AdventOfCode.ProgressTracker")?.GetMethod("Stop", BindingFlags.Public | BindingFlags.Static);

var solversByPuzzle = aocAssembly
    .GetTypes()
    .Where(t => t.IsSubclassOf(baseSolverType) && !t.IsAbstract)
    .Where(t => !excluded.Contains(t.FullName ?? t.Name) && !excluded.Contains(t.Name))
    .Select(t =>
    {
        var instance = Activator.CreateInstance(t)!;
        int year = (int)yearProp.GetValue(instance)!;
        int day = (int)dayProp.GetValue(instance)!;
        return (Type: t, Instance: instance, Year: year, Day: day);
    })
    .GroupBy(x => (x.Year, x.Day))
    .ToDictionary(
        g => g.Key,
        g => g.OrderBy(x => x.Type.Name, StringComparer.Ordinal).ToList());

var partTimeout = TimeSpan.FromSeconds(30);
int totalChecks = 0;
int passedChecks = 0;
int failedChecks = 0;
var summaryRows = new List<string>();

foreach (var (key, expected) in config.Puzzles.OrderBy(kv => kv.Key, StringComparer.Ordinal))
{
    var parts = key.Split('-');
    if (parts.Length != 2 || !int.TryParse(parts[0], out int year) || !int.TryParse(parts[1], out int day))
    {
        Console.Error.WriteLine($"[FAIL] Invalid puzzle key '{key}'. Expected YYYY-DD.");
        failedChecks++;
        continue;
    }

    var inputPath = Path.Combine(inputsRoot, year.ToString(), $"day{day:D2}.txt");
    if (!File.Exists(inputPath))
    {
        Console.Error.WriteLine($"[FAIL] {key}: Input file missing at {inputPath}");
        failedChecks++;
        summaryRows.Add($"| `{key}` | - | FAIL | Input missing |");
        continue;
    }

    if (!solversByPuzzle.TryGetValue((year, day), out var solvers) || solvers.Count == 0)
    {
        Console.Error.WriteLine($"[FAIL] {key}: No non-excluded solver found.");
        failedChecks++;
        summaryRows.Add($"| `{key}` | - | FAIL | No solver found |");
        continue;
    }

    var input = File.ReadAllLines(inputPath);

    foreach (var (solverType, solver, _, _) in solvers)
    {
        var solverName = solverType.Name;
        var originalOut = Console.Out;
        using var captured = new StringWriter();
        Console.SetOut(captured);

        var sw = Stopwatch.StartNew();
        var (p1Ok, p1Msg) = VerifyPart(1, expected.Part1, () => InvokeSolver(part1Method, solver, input), partTimeout);
        var (p2Ok, p2Msg) = VerifyPart(2, expected.Part2, () => InvokeSolver(part2Method, solver, input), partTimeout);
        sw.Stop();

        stopProgressMethod?.Invoke(null, null);
        Console.SetOut(originalOut);

        bool allOk = p1Ok && p2Ok;
        totalChecks++;
        if (allOk)
        {
            passedChecks++;
            Console.WriteLine($"[PASS] {key} {solverName} ({p1Msg}, {p2Msg}) [{sw.ElapsedMilliseconds} ms]");
            summaryRows.Add($"| `{key}` | `{solverName}` | PASS | {p1Msg}; {p2Msg} ({sw.ElapsedMilliseconds} ms) |");
        }
        else
        {
            failedChecks++;
            Console.Error.WriteLine($"[FAIL] {key} {solverName} ({p1Msg}, {p2Msg}) [{sw.ElapsedMilliseconds} ms]");
            var stdout = captured.ToString().Trim();
            if (!string.IsNullOrEmpty(stdout))
            {
                Console.Error.WriteLine($"  Captured stdout:\n{stdout}");
            }
            summaryRows.Add($"| `{key}` | `{solverName}` | FAIL | {p1Msg}; {p2Msg} |");
        }
    }
}

Console.WriteLine();
Console.WriteLine($"Verified {totalChecks} solver(s): {passedChecks} passed, {failedChecks} failed.");
if (excluded.Count > 0)
{
    Console.WriteLine($"Excluded solvers ({excluded.Count}): {string.Join(", ", excluded.Order())}");
}

var stepSummaryPath = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
if (!string.IsNullOrWhiteSpace(stepSummaryPath))
{
    var sb = new StringBuilder();
    sb.AppendLine("## Advent of Code Ground Truth Verification");
    sb.AppendLine();
    sb.AppendLine($"- **Verified solvers:** {totalChecks}");
    sb.AppendLine($"- **Passed:** {passedChecks}");
    sb.AppendLine($"- **Failed:** {failedChecks}");
    sb.AppendLine();
    sb.AppendLine("| Puzzle | Solver | Status | Details |");
    sb.AppendLine("| --- | --- | --- | --- |");
    foreach (var row in summaryRows)
    {
        sb.AppendLine(row);
    }
    File.AppendAllText(stepSummaryPath, sb.ToString());
}

return failedChecks == 0 ? 0 : 1;

static string InvokeSolver(MethodInfo method, object instance, string[] input)
{
    try
    {
        return (string?)method.Invoke(instance, [input]) ?? string.Empty;
    }
    catch (TargetInvocationException ex) when (ex.InnerException is not null)
    {
        throw ex.InnerException;
    }
}

static (bool Ok, string Message) VerifyPart(int partNumber, string? expected, Func<string> run, TimeSpan timeout)
{
    if (expected is null)
    {
        return (true, $"Part {partNumber}: skipped");
    }

    try
    {
        var task = Task.Run(run);
        if (!task.Wait(timeout))
        {
            return (false, $"Part {partNumber}: TIMEOUT after {timeout.TotalSeconds:0}s (expected '{expected}')");
        }

        var actual = task.Result;
        if (actual == expected)
        {
            return (true, $"Part {partNumber}: {actual}");
        }

        return (false, $"Part {partNumber}: expected '{expected}', got '{actual}'");
    }
    catch (AggregateException ex) when (ex.InnerException is not null)
    {
        return (false, $"Part {partNumber}: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
    }
    catch (Exception ex)
    {
        return (false, $"Part {partNumber}: {ex.GetType().Name}: {ex.Message}");
    }
}

sealed class PluginLoadContext(string pluginPath) : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver = new(pluginPath);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        return assemblyPath is not null ? LoadFromAssemblyPath(assemblyPath) : null;
    }

    protected override nint LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return libraryPath is not null ? LoadUnmanagedDllFromPath(libraryPath) : nint.Zero;
    }
}

sealed record GroundTruthConfig(
    [property: JsonPropertyName("excludeSolvers")] List<string>? ExcludeSolvers,
    [property: JsonPropertyName("puzzles")] Dictionary<string, PuzzleGroundTruth> Puzzles);

sealed record PuzzleGroundTruth(
    [property: JsonPropertyName("part1")] string? Part1,
    [property: JsonPropertyName("part2")] string? Part2);
