using NUnit.Framework;

namespace AdventOfCode.Year2015;

public class Day23 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 23;

    struct Instruction
    {
        public string OpCode { get; set; }
        public char Register { get; set; }
        public int Offset { get; set; }
    }


    List<Instruction> Parse(string[] input)
    {
        List<Instruction> instructions = new List<Instruction>();
        for (int i = 0; i < input.Length; i++)
        {
            string line = input[i];
            string[] splits = line.Split(' ');
            Instruction instruction = new();
            if (splits.Length == 2)
            {
                instruction.OpCode = splits[0];
                if (instruction.OpCode == "jmp")
                    instruction.Offset = int.Parse(splits[1]);
                else
                    instruction.Register = splits[1][0];
            }
            else
            {
                instruction.OpCode = splits[0];
                instruction.Register = splits[1][0];
                instruction.Offset = int.Parse(splits[2]);
            }

            instructions.Add(instruction);
        }

        return instructions;
    }

    int Run(List<Instruction> instructions, int a, int b)
    {
        int rip = 0;

        while (rip < instructions.Count)
        {
            var instruction = instructions[rip];

            Assert.IsTrue(a >= 0);
            Assert.IsTrue(b >= 0);
            switch (instruction.OpCode)
            {
                case "hlf":
                    if (instruction.Register == 'a')
                        a /= 2;
                    else
                        b /= 2;
                    rip++;
                    break;
                case "tpl":
                    if (instruction.Register == 'a')
                        a *= 3;
                    else
                        b *= 3;
                    rip++;
                    break;
                case "inc":
                    if (instruction.Register == 'a')
                        a += 1;
                    else
                        b += 1;
                    rip++;
                    break;
                case "jmp":
                    rip += instruction.Offset;
                    break;
                case "jie":
                    if (instruction.Register == 'a' && a % 2 == 0)
                        rip += instruction.Offset;
                    else if (instruction.Register == 'b' && b % 2 == 0)
                        rip += instruction.Offset;
                    else
                        rip++;

                    break;
                case "jio":
                    if (instruction.Register == 'a' && a == 1)
                        rip += instruction.Offset;
                    else if (instruction.Register == 'b' && b == 1)
                        rip += instruction.Offset;
                    else
                        rip++;
                    break;
                default:
                    throw new Exception("illegal instruction");
                    break;
            }
        }

        return b;
    }

    public override string SolvePart1(string[] input)
    {
        List<Instruction> instructions = Parse(input);
        int b = Run(instructions, 0, 0);

        return b.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        List<Instruction> instructions = Parse(input);
        int b = Run(instructions, 1, 0);

        return b.ToString();
        return "Not implemented yet";
    }
}