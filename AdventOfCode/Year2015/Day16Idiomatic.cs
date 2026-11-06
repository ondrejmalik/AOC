namespace AdventOfCode.Year2015;

public class Day16Idiomatic : BaseSolver
{
    //children: 3
//cats: 7
//samoyeds: 2
//pomeranians: 3
//akitas: 0
//vizslas: 0
//goldfish: 5
//trees: 3
//cars: 2
//perfumes: 1
    record Sue(int Number, Dictionary<string, int> Props);


    public override int Year => 2015;
    public override int Day => 16;

    List<Sue> Parse(string[] lines)
    {
        List<Sue> sues = new(lines.Length);

        foreach ((var i, var line) in lines.Index())
        {
            Sue sue = new Sue(i, new Dictionary<string, int>());
            string propertiesPart = line.Substring(line.IndexOf(':') + 1);

            foreach (string item in propertiesPart.Split(','))
            {
                string[] pair = item.Split(':');
                string key = pair[0].Trim();
                int value = int.Parse(pair[1].Trim());
                sue.Props.Add(key, value);
            }

            sues.Add(sue);
        }

        return sues;
    }

    public override string SolvePart1(string[] input)
    {
        Sue ourSue = new Sue(0, new Dictionary<string, int>
        {
            ["children"] = 3,
            ["cats"] = 7,
            ["samoyeds"] = 2,
            ["pomeranians"] = 3,
            ["akitas"] = 0,
            ["vizslas"] = 0,
            ["goldfish"] = 5,
            ["trees"] = 3,
            ["cars"] = 2,
            ["perfumes"] = 1
        });

        List<Sue> sues = Parse(input);
        return (sues.First(sue => sue.Props.All(kv => ourSue.Props[kv.Key] == kv.Value)).Number + 1).ToString();
    }

    public override string SolvePart2(string[] input)
    {
        Sue ourSue = new Sue(0, new Dictionary<string, int>
        {
            ["children"] = 3,
            ["cats"] = 7,
            ["samoyeds"] = 2,
            ["pomeranians"] = 3,
            ["akitas"] = 0,
            ["vizslas"] = 0,
            ["goldfish"] = 5,
            ["trees"] = 3,
            ["cars"] = 2,
            ["perfumes"] = 1
        });

        List<Sue> sues = Parse(input);

        return (sues.First(sue => sue.Props.All(kv => kv.Key switch
        {
            "cats" or "trees" => kv.Value > ourSue.Props[kv.Key],
            "pomeranians" or "goldfish" => kv.Value < ourSue.Props[kv.Key],
            _ => kv.Value == ourSue.Props[kv.Key]
        })).Number + 1).ToString();
    }
} // tady jsem to chctel nejdriv delat pres .Method() ale c# nema .ForEach() pro IEunmerable tazke bych musel delat Alokace takze se to muselo prepsat imperativne
// hned na zacatku jsem si uvedomil ze musim pridat 1 k indexu ale pak jsem na to zapomel takze jsem jako odpoved dostal off by 1 error

/* IDIOMATIOC
gemini rikal at z properties udelam dicitonary tak jsem to tak udelal trochu to zkratilo kod*/