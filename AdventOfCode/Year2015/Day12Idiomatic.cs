using System.Text.Json;

namespace AdventOfCode.Year2015;

public class Day12Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 12;

//ignore red flag is for part2
    int Sum(JsonElement el, bool ignoreRed) => el.ValueKind switch
    {
        JsonValueKind.Number => el.GetInt32(),
        JsonValueKind.Array => el.EnumerateArray().Sum(child => Sum(child, ignoreRed)),
        JsonValueKind.Object when ignoreRed && el.EnumerateObject()
            .Any(p => p.Value.ValueKind == JsonValueKind.String && p.Value.ValueEquals("red")) => 0,
        JsonValueKind.Object => el.EnumerateObject().Sum(p => Sum(p.Value, ignoreRed)),
        _ => 0
    };

    public override string SolvePart1(string[] input)
    {
        var a = JsonDocument.Parse(input[0]);
        var current = a.RootElement;
        int total = 0;


        return $"{Sum(current, false)}";
    }

    public override string SolvePart2(string[] input)
    {
        var a = JsonDocument.Parse(input[0]);
        var current = a.RootElement;
        return $"{Sum(current, true)}";
    }
}
/* IDIOMATIC
 odstraneni hodne duplicit kde se ai zbavilo duplicitniho kodu a pridani flag ignoreRed ktery je pro part2
 */