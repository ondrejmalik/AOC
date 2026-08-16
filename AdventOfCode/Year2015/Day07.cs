using System.Numerics;

namespace AdventOfCode.Year2015;

public class Day07 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 7;

    public enum OpCode
    {
        AND,
        OR,
        LSHIFT,
        RSHIFT,
        NOT,
        COPY
    }

    Dictionary<string, (OpCode, string, string)> WireDefinition(string[] input)
    {
        Dictionary<string, (OpCode, string, string)> dictionary = new();
        foreach (string line in input)
        {
            string[] pair = line.Split(" -> ");

            string logic = pair[0];
            string location = pair[1];
            OpCode opCode;
            string op1;
            string op2 = "";
            switch (logic)
            {
                case var s when s.Contains("AND"):
                {
                    var operandLocations = s.Split(" AND ");
                    opCode = OpCode.AND;
                    op1 = operandLocations[0];
                    op2 = operandLocations[1];
                    break;
                }
                case var s when s.Contains("OR"):
                {
                    var operandLocations = s.Split(" OR ");

                    opCode = OpCode.OR;
                    op1 = operandLocations[0];
                    op2 = operandLocations[1];
                    break;
                }

                case var s when s.Contains("LSHIFT"):
                {
                    var operandLocations = s.Split(" LSHIFT ");

                    opCode = OpCode.LSHIFT;
                    op1 = operandLocations[0];
                    op2 = operandLocations[1];
                    break;
                }

                case var s when s.Contains("RSHIFT"):
                {
                    var operandLocations = s.Split(" RSHIFT ");

                    opCode = OpCode.RSHIFT;
                    op1 = operandLocations[0];
                    op2 = operandLocations[1];
                    break;
                }

                case var s when s.Contains("NOT"):
                {
                    var operandLocations = s.Split("NOT ");

                    opCode = OpCode.NOT;
                    op1 = operandLocations[1];
                    break;
                }
                case var s:
                {
                    opCode = OpCode.COPY;
                    op1 = s;
                    break;
                }
            }

            dictionary.Add(location, (opCode, op1, op2));
        }

        return dictionary;
    }

    ushort RunLogic(Dictionary<string, (OpCode, string, string)> dictionary, Dictionary<string, ushort> cache,
        string neededLocation)
    {
        ushort numberLiteral;
        if (ushort.TryParse(neededLocation, out numberLiteral))
        {
            ProgressTracker.Increment();
            return numberLiteral;
        }

        var ops = dictionary[neededLocation];
        OpCode opCode = ops.Item1;
        string op1 = ops.Item2;
        string op2 = ops.Item3;

        switch (opCode)
        {
            case OpCode.AND:
            {
                ushort op1Val;
                if (!cache.TryGetValue(op1, out op1Val))
                {
                    op1Val = RunLogic(dictionary, cache, op1);
                    cache[op1] = op1Val;
                }

                ushort op2Val;
                if (!cache.TryGetValue(op2, out op2Val))
                {
                    op2Val = RunLogic(dictionary, cache, op2);
                    cache[op2] = op2Val;
                }

                op1Val &= op2Val;
                return op1Val;
            }
            case OpCode.OR:
            {
                ushort op1Val;
                if (!cache.TryGetValue(op1, out op1Val))
                {
                    op1Val = RunLogic(dictionary, cache, op1);
                    cache[op1] = op1Val;
                }

                ushort op2Val;
                if (!cache.TryGetValue(op2, out op2Val))
                {
                    op2Val = RunLogic(dictionary, cache, op2);
                    cache[op2] = op2Val;
                }

                op1Val |= op2Val;
                return op1Val;
            }
            case OpCode.LSHIFT:
            {
                ushort op1Val;
                if (!cache.TryGetValue(op1, out op1Val))
                {
                    op1Val = RunLogic(dictionary, cache, op1);
                    cache[op1] = op1Val;
                }

                ushort op2Val;
                if (!cache.TryGetValue(op2, out op2Val))
                {
                    op2Val = ushort.Parse(op2);
                    cache[op2] = op2Val;
                }

                op1Val <<= op2Val;
                return op1Val;
            }
            case OpCode.RSHIFT:
            {
                ushort op1Val;
                if (!cache.TryGetValue(op1, out op1Val))
                {
                    op1Val = RunLogic(dictionary, cache, op1);
                    cache[op1] = op1Val;
                }

                ushort op2Val;
                if (!cache.TryGetValue(op2, out op2Val))
                {
                    op2Val = ushort.Parse(op2);
                    cache[op2] = op2Val;
                }

                op1Val >>= op2Val;
                return op1Val;
            }
            case OpCode.NOT:
            {
                ushort op1Val;
                if (!cache.TryGetValue(op1, out op1Val))
                {
                    op1Val = RunLogic(dictionary, cache, op1);
                    cache[op1] = op1Val;
                }

                return (ushort)~op1Val;
            }
            case OpCode.COPY:
            {
                ushort op1Val;
                if (!cache.TryGetValue(op1, out op1Val))
                {
                    if (!ushort.TryParse(op1, out op1Val))
                    {
                        op1Val = RunLogic(dictionary, cache, op1);
                    }

                    cache[op1] = op1Val;
                }

                return op1Val;
            }
        }

        return 0;
    }


    public override string SolvePart1(string[] input)
    {
        ProgressTracker.Start(message: "Searched paths: ", updateIntervalMs: 1000);

        Dictionary<string, ushort> cache = new();
        Dictionary<string, (OpCode, string, string)> dictionary = WireDefinition(input);
        ushort aVal = RunLogic(dictionary, cache, "a");
        Console.WriteLine(aVal);


        return aVal.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        ProgressTracker.Start(message: "Searched paths: ", updateIntervalMs: 1000);

        Dictionary<string, ushort> cache = new();
        Dictionary<string, (OpCode, string, string)> dictionary = WireDefinition(input);
        cache["b"] = 16076; // Part1 solution
        ushort aVal = RunLogic(dictionary, cache, "a");
        Console.WriteLine(aVal);


        return aVal.ToString();
    }
}
/*notes
 tohle bylo celkem tezky
 nejdriv jsem si myslel ze to je jako asembly a mam to poustet postupne instrukci po instrukci
 spravne to ale bylo tak ze musim zjistit co potrebuju k cemu (abych mohl zjistit vystup musim znat hodnoty operandu
 to jsem udelal pres zpetne hledani rekurzivne. Nejdriv jsem vzal operand a pak jsem ho dal jako needed location a prohledaval cely soubor
 to bylo ale pomaly a nenaslo to ani za hodinu
 pak jsem pridal dictionary - nejdriv jsem si myslel ze mam udelat operandy opcode -> vysledek ale vezkutecnosti to ma byt naopak protoze rekurzivni funkce hleda podle needed location a to je vysledek
 to ale nestacilo a bylo potreba pridat cachovani vysledku nejriv jsem to chtel dat do toho samyho dictionary ale pak jsem se rozhodl dat to zvlast jako <string,ushort>
 to se nakonec osvedcilo protoze ve druhy casti stacilo dat do cache vysledek z prvni casti do cache b
*/