namespace AdventOfCode.Year2015;

public union Operand(ushort, string);

public closed record Gate;

public sealed record UnaryGate(string Op, Operand Src) : Gate;

public sealed record BinaryGate(string Op, Operand Left, Operand Right) : Gate;

public class Day07Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 7;

    private static Operand ParseOperand(string token) =>
        ushort.TryParse(token, out var literal) ? literal : token;

    private static Dictionary<string, Gate> ParseCircuit(string[] input)
    {
        Dictionary<string, Gate> gates = new(capacity: input.Length);

        foreach (var line in input)
        {
            string[] parts = line.Split(' ');
            string target = parts[^1];

            Gate gate = parts switch
            {
                [var src, "->", _] =>
                    new UnaryGate("COPY", ParseOperand(src)),

                ["NOT", var src, "->", _] =>
                    new UnaryGate("NOT", ParseOperand(src)),

                [var left, var op, var right, "->", _] =>
                    new BinaryGate(op, ParseOperand(left), ParseOperand(right)),

                _ => throw new FormatException($"Invalid circuit instruction: {line}")
            };

            gates[target] = gate;
        }

        return gates;
    }

    private static ushort EvaluateCircuit(
        Dictionary<string, Gate> gates,
        Dictionary<string, ushort> cache,
        string targetWire)
    {
        return EvaluateWire(targetWire);

        ushort Resolve(Operand operand) => operand.Value switch
        {
            ushort literal => literal,
            string wire => EvaluateWire(wire),
            _ => throw new InvalidOperationException("Uninitialized Operand")
        };

        ushort EvaluateWire(string wire)
        {
            if (cache.TryGetValue(wire, out ushort cached))
                return cached;

            ProgressTracker.Increment();

            ushort result = gates[wire] switch
            {
                UnaryGate(var op, var src) => op switch
                {
                    "COPY" => Resolve(src),
                    "NOT" => (ushort)~Resolve(src),
                    _ => throw new InvalidOperationException($"Unknown unary op '{op}'")
                },
                BinaryGate(var op, var left, var right) => op switch
                {
                    "AND" => (ushort)(Resolve(left) & Resolve(right)),
                    "OR" => (ushort)(Resolve(left) | Resolve(right)),
                    "LSHIFT" => (ushort)(Resolve(left) << Resolve(right)),
                    "RSHIFT" => (ushort)(Resolve(left) >> Resolve(right)),
                    _ => throw new InvalidOperationException($"Unknown binary op '{op}'")
                }
            };

            cache[wire] = result;
            return result;
        }
    }

    public override string SolvePart1(string[] input)
    {
        ProgressTracker.Start(message: "Evaluated gates: ", updateIntervalMs: 1000);

        Dictionary<string, Gate> gates = ParseCircuit(input);
        Dictionary<string, ushort> cache = new(capacity: gates.Count);

        ushort aVal = EvaluateCircuit(gates, cache, "a");
        return aVal.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        ProgressTracker.Start(message: "Evaluated gates: ", updateIntervalMs: 1000);

        Dictionary<string, Gate> gates = ParseCircuit(input);
        Dictionary<string, ushort> cache = new(capacity: gates.Count);

        // Dynamically compute Part 1's value and override wire "b" (no hardcoded constants!)
        ushort part1A = EvaluateCircuit(gates, cache, "a");

        cache.Clear();
        cache["b"] = part1A;

        ushort aVal = EvaluateCircuit(gates, cache, "a");
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
 prej to jeste slo udelat pres topological sort ale to nevim jak
*/
/*IDIOMATIC
Tohle jsem si nechal vygenerovat od ai pouziva to union ktery je az od net11 jako literal to pouziva ushort
Operand muze byt bud literal cislo nebo string token ktery se musi evaluovat
Gate je closed record a dedi od ni Unary a Binary gate
libi se mi ze funkce rezolve je napsana ve funkci EvaluateCircuit takze venku neni videt
ve funkci evaluate wire se pak pozna jestli jde o unary nebo binary gate a ty se pak switchujou podle toho jaky jsou u nich povoleny operandy
druhou cast udela chytre tak ze vycisti cache a do ty nastavi part1A takze se zbavi konstanty ja jsem to tam napsal natvrdo
*/