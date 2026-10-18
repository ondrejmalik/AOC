using System.Text.Json;

namespace AdventOfCode.Year2015;

public class Day12 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 12;


    int Iterate(JsonElement.ArrayEnumerator array)
    {
        int total = 0;
        foreach (var element in array)
        {
            total += Select(element);
        }

        return total;
    }

    int Iterate(JsonElement.ObjectEnumerator obj)
    {
        int total = 0;

        foreach (var element in obj)
        {
            total += Select(element);
        }

        return total;
    }


    int Select(JsonElement current)
    {
        int total = 0;
        switch (current.ValueKind)
        {
            case JsonValueKind.Array:
                JsonElement.ArrayEnumerator array = current.EnumerateArray();
                total += Iterate(array);
                break;
            case JsonValueKind.Object:
                JsonElement.ObjectEnumerator obj = current.EnumerateObject();
                total += Iterate(obj);
                break;
            case JsonValueKind.String:
                string str = current.GetString()!;
                int num = 0;
                _ = Int32.TryParse(str, out num);
                total += num;

                break;
            case JsonValueKind.Number:
                total += current.GetInt32();
                break;
        }

        return total;
    }

    int Select(JsonProperty current)
    {
        int total = 0;

        switch (current.Value.ValueKind)
        {
            case JsonValueKind.Array:
                JsonElement.ArrayEnumerator array = current.Value.EnumerateArray();
                total += Iterate(array);
                break;
            case JsonValueKind.Object:
                JsonElement.ObjectEnumerator obj = current.Value.EnumerateObject();
                total += Iterate(obj);
                break;
            case JsonValueKind.String:
                string str = current.Value.GetString()!;
                int num = 0;
                _ = Int32.TryParse(str, out num);
                total += num;
                break;
            case JsonValueKind.Number:
                total += current.Value.GetInt32();
                break;
        }

        return total;
    }

    public override string SolvePart1(string[] input)
    {
        var a = JsonDocument.Parse(input[0]);
        var current = a.RootElement;
        int total = 0;

        total += Select(current);

        return $"{total}";
    }

    public override string SolvePart2(string[] input)
    {
        var a = JsonDocument.Parse(input[0]);
        var current = a.RootElement;
        int total = 0;

        total += SelectPart2(current);

        return $"{total}";
    }


    int IteratePart2(JsonElement.ArrayEnumerator array)
    {
        int total = 0;
        foreach (var element in array)
        {
            total += SelectPart2(element);
        }

        return total;
    }

    int IteratePart2(JsonElement.ObjectEnumerator obj)
    {
        int total = 0;
        bool containsRed = false;
        foreach (var element in obj)
        {
            if (element.Value.ToString() == "red")
            {
                containsRed = true;
                break;
            }

            total += SelectPart2(element);
        }

        if (containsRed)
            return 0;

        return total;
    }


    int SelectPart2(JsonElement current)
    {
        int total = 0;
        switch (current.ValueKind)
        {
            case JsonValueKind.Array:
                JsonElement.ArrayEnumerator array = current.EnumerateArray();
                total += IteratePart2(array);
                break;
            case JsonValueKind.Object:
                JsonElement.ObjectEnumerator obj = current.EnumerateObject();

                total += IteratePart2(obj);
                break;
            case JsonValueKind.String:
                string str = current.GetString()!;
                int num = 0;
                _ = Int32.TryParse(str, out num);
                total += num;

                break;
            case JsonValueKind.Number:
                total += current.GetInt32();
                break;
        }

        return total;
    }

    int SelectPart2(JsonProperty current)
    {
        int total = 0;

        switch (current.Value.ValueKind)
        {
            case JsonValueKind.Array:
                JsonElement.ArrayEnumerator array = current.Value.EnumerateArray();
                total += IteratePart2(array);
                break;
            case JsonValueKind.Object:
                JsonElement.ObjectEnumerator obj = current.Value.EnumerateObject();
                total += IteratePart2(obj);
                break;
            case JsonValueKind.String:
                string str = current.Value.GetString()!;
                int num = 0;
                _ = Int32.TryParse(str, out num);
                total += num;
                break;
            case JsonValueKind.Number:
                total += current.Value.GetInt32();
                break;
        }

        return total;
    }
}