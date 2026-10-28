namespace AdventOfCode.Year2015;

public class Day16 : BaseSolver
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
    class Sue
    {
        public int Children { get; set; } = -1;
        public int Cats { get; set; } = -1;
        public int Samoyeds { get; set; } = -1;
        public int Pomeranians { get; set; } = -1;
        public int Akitas { get; set; } = -1;
        public int Vizslas { get; set; } = -1;
        public int Goldfish { get; set; } = -1;
        public int Trees { get; set; } = -1;
        public int Cars { get; set; } = -1;
        public int Perfumes { get; set; } = -1;
    }

    public override int Year => 2015;
    public override int Day => 16;

    List<Sue> Parse(string[] lines)
    {
        List<Sue> sues = new(lines.Length);

        foreach (string line in lines)
        {
            Sue sue = new Sue();
            string propertiesPart = line.Substring(line.IndexOf(':') + 1);

            foreach (string item in propertiesPart.Split(','))
            {
                string[] pair = item.Split(':');
                string key = pair[0].Trim();
                int value = int.Parse(pair[1].Trim());

                switch (key)
                {
                    case "children": sue.Children = value; break;
                    case "cats": sue.Cats = value; break;
                    case "samoyeds": sue.Samoyeds = value; break;
                    case "pomeranians": sue.Pomeranians = value; break;
                    case "akitas": sue.Akitas = value; break;
                    case "vizslas": sue.Vizslas = value; break;
                    case "goldfish": sue.Goldfish = value; break;
                    case "trees": sue.Trees = value; break;
                    case "cars": sue.Cars = value; break;
                    case "perfumes": sue.Perfumes = value; break;
                }
            }

            sues.Add(sue);
        }

        return sues;
    }

    public override string SolvePart1(string[] input)
    {
        Sue ourSue = new Sue()
        {
            Children = 3,
            Cats = 7,
            Samoyeds = 2,
            Pomeranians = 3,
            Akitas = 0,
            Vizslas = 0,
            Goldfish = 5,
            Trees = 3,
            Cars = 2,
            Perfumes = 1
        };

        List<Sue> sues = Parse(input);
        var suesRes = sues.Where(sue =>
        {
            return (sue.Children == ourSue.Children || sue.Children == -1) &&
                   (sue.Cats == ourSue.Cats || sue.Cats == -1) &&
                   (sue.Samoyeds == ourSue.Samoyeds || sue.Samoyeds == -1) &&
                   (sue.Pomeranians == ourSue.Pomeranians || sue.Pomeranians == -1) &&
                   (sue.Akitas == ourSue.Akitas || sue.Akitas == -1) &&
                   (sue.Vizslas == ourSue.Vizslas || sue.Vizslas == -1) &&
                   (sue.Goldfish == ourSue.Goldfish || sue.Goldfish == -1) &&
                   (sue.Trees == ourSue.Trees || sue.Trees == -1) &&
                   (sue.Cars == ourSue.Cars || sue.Cars == -1) &&
                   (sue.Perfumes == ourSue.Perfumes || sue.Perfumes == -1);
        }).ToString();

        var index = Int32.Parse(sues.FindIndex(sue =>
        {
            return (sue.Children == ourSue.Children || sue.Children == -1) &&
                   (sue.Cats == ourSue.Cats || sue.Cats == -1) &&
                   (sue.Samoyeds == ourSue.Samoyeds || sue.Samoyeds == -1) &&
                   (sue.Pomeranians == ourSue.Pomeranians || sue.Pomeranians == -1) &&
                   (sue.Akitas == ourSue.Akitas || sue.Akitas == -1) &&
                   (sue.Vizslas == ourSue.Vizslas || sue.Vizslas == -1) &&
                   (sue.Goldfish == ourSue.Goldfish || sue.Goldfish == -1) &&
                   (sue.Trees == ourSue.Trees || sue.Trees == -1) &&
                   (sue.Cars == ourSue.Cars || sue.Cars == -1) &&
                   (sue.Perfumes == ourSue.Perfumes || sue.Perfumes == -1);
        }).ToString());

        return (index + 1).ToString();
    }

    public override string SolvePart2(string[] input)
    {
        Sue ourSue = new Sue()
        {
            Children = 3,
            Cats = 7,
            Samoyeds = 2,
            Pomeranians = 3,
            Akitas = 0,
            Vizslas = 0,
            Goldfish = 5,
            Trees = 3,
            Cars = 2,
            Perfumes = 1
        };

        List<Sue> sues = Parse(input);
        var suesRes = sues.Where(sue =>
        {
            return (sue.Children == ourSue.Children || sue.Children == -1) &&
                   (sue.Cats == ourSue.Cats || sue.Cats == -1) &&
                   (sue.Samoyeds == ourSue.Samoyeds || sue.Samoyeds == -1) &&
                   (sue.Pomeranians == ourSue.Pomeranians || sue.Pomeranians == -1) &&
                   (sue.Akitas == ourSue.Akitas || sue.Akitas == -1) &&
                   (sue.Vizslas == ourSue.Vizslas || sue.Vizslas == -1) &&
                   (sue.Goldfish == ourSue.Goldfish || sue.Goldfish == -1) &&
                   (sue.Trees == ourSue.Trees || sue.Trees == -1) &&
                   (sue.Cars == ourSue.Cars || sue.Cars == -1) &&
                   (sue.Perfumes == ourSue.Perfumes || sue.Perfumes == -1);
        }).ToString();
        var index = Int32.Parse(sues.FindIndex(sue =>
        {
            return (sue.Children == ourSue.Children || sue.Children == -1) &&
                   (sue.Cats > ourSue.Cats || sue.Cats == -1) &&
                   (sue.Samoyeds == ourSue.Samoyeds || sue.Samoyeds == -1) &&
                   (sue.Pomeranians < ourSue.Pomeranians || sue.Pomeranians == -1) &&
                   (sue.Akitas == ourSue.Akitas || sue.Akitas == -1) &&
                   (sue.Vizslas == ourSue.Vizslas || sue.Vizslas == -1) &&
                   (sue.Goldfish < ourSue.Goldfish || sue.Goldfish == -1) &&
                   (sue.Trees >= ourSue.Trees || sue.Trees == -1) &&
                   (sue.Cars == ourSue.Cars || sue.Cars == -1) &&
                   (sue.Perfumes == ourSue.Perfumes || sue.Perfumes == -1);
        }).ToString());


        return (index + 1).ToString();
    }
}// tady jsem to chctel nejdriv delat pres .Method() ale c# nema .ForEach() pro IEunmerable tazke bych musel delat Alokace takze se to muselo prepsat imperativne
// hned na zacatku jsem si uvedomil ze musim pridat 1 k indexu ale pak jsem na to zapomel takze jsem jako odpoved dostal off by 1 error