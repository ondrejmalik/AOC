namespace AdventOfCode.Year2015;

public class Day09 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 9;

    public class Node
    {
        public string Name { get; set; }
        public List<(Node location, int distance)> Edges { get; set; } = new();
    }

    public class Graph
    {
        public HashSet<Node> Nodes { get; set; } = new();
        public int MinLength { get; set; } = Int32.MaxValue;

        public int MaxLength { get; set; } = Int32.MinValue;

        public Node GetOrCreate(string name)
        {
            var node = Nodes.FirstOrDefault(n => n.Name == name);
            if (node is null)
            {
                node = new Node { Name = name };
                Nodes.Add(node);
            }

            return node;
        }

        public void TraverseMinimal(Node node, int sumLength, List<Node> unvisited)
        {
            unvisited.Remove(node);
            if (unvisited.Count == 0)
            {
                //Console.WriteLine(sumLength);
                MinLength = Math.Min(MinLength, sumLength);
            }

            foreach (var edge in node.Edges)
            {
                //already visited
                if (!unvisited.Contains(edge.location))
                    continue;
                TraverseMinimal(edge.location, sumLength + edge.distance, unvisited.ToList());
            }
        }

        public void TraverseMaximal(Node node, int sumLength, List<Node> unvisited)
        {
            unvisited.Remove(node);
            if (unvisited.Count == 0)
            {
                //Console.WriteLine(sumLength);
                MaxLength = Math.Max(MaxLength, sumLength);
            }

            foreach (var edge in node.Edges)
            {
                //already visited
                if (!unvisited.Contains(edge.location))
                    continue;
                TraverseMaximal(edge.location, sumLength + edge.distance, unvisited.ToList());
            }
        }
    }


    public override string SolvePart1(string[] input)
    {
        Graph graph = new Graph();
        foreach (string line in input)
        {
            string[] split = line.Split(" = ");
            string[] fromTo = split[0].Split(" to ");

            int distance = Int32.Parse(split[1]);

            Node from = graph.GetOrCreate(fromTo[0]);
            Node to = graph.GetOrCreate(fromTo[1]);

            from.Edges.Add((to, distance));
            to.Edges.Add((from, distance));
        }

        foreach (Node node in graph.Nodes)
        {
            List<Node> unvisited = graph.Nodes.ToList();
            int sumLength = 0;
            graph.TraverseMinimal(node, sumLength, unvisited);
        }

        return graph.MinLength.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        Graph graph = new Graph();
        foreach (string line in input)
        {
            string[] split = line.Split(" = ");
            string[] fromTo = split[0].Split(" to ");

            int distance = Int32.Parse(split[1]);

            Node from = graph.GetOrCreate(fromTo[0]);
            Node to = graph.GetOrCreate(fromTo[1]);

            from.Edges.Add((to, distance));
            to.Edges.Add((from, distance));
        }

        foreach (Node node in graph.Nodes)
        {
            List<Node> unvisited = graph.Nodes.ToList();
            int sumLength = 0;
            graph.TraverseMaximal(node, sumLength, unvisited);
        }

        return graph.MaxLength.ToString();
    }
}
/* note
 tohle mi delalo problem celkem jsem to delal pres 3 hodiny ale mezitim jsem se sel umyt
 na zacatku jsem nevedel jak mam udelat graf jestli udelat class graf s List<verticie> a i List<edge>
 ale pak me napadlo ze logicky to je vlastne linked list s vice dalsima nodama takze jsem nakonec vzal Node a do toho nacpal list<node>
 a v grafu je taky List<node> napadlo me ze se to da delat rekurzivne a neni to djikstra protoze ten nenavstivi kazdy vertex ani kostra grafu
 problem mi delaly reference a vytvareni objektu DULEZITE je od ted nejdriv kontrolovat jestli ten objekt existuje a pak az ho vytvorit ja to delal naopak a musel se zeptat gemini
 dulezity bylo si vedomit ze edge je oboustrana takze londyn dublin funguje i jako dublin londyn
 pak uz stacilo jen projit kazdy node a z toho rekurzivne prohledavat kazdy edge
 ale jen ty nody ktery jsou v unchecked listu.
 ten bylo treba pres .ToList() prekopirovat aby to nebyla ta sama reference 
 kdyz uz byl unchecked prazdny mohl jsem vypsat vysledek a ulozit ho do public promeny MinLength
 docela fanj bylo dat Traverse metodu do tridy graf aby ho videlaa mohla pouzivat MinLength
 dobra optimalizace od gemini by byla zmenit HashSet na Dictionary aby v metode GetOrCreate nemusela prohledavat cely Set a klic by byl string nazvu mesta treba "londyn",Node - "Londyn" - 3 Edges
 
*/