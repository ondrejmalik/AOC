using NUnit.Framework;

namespace AdventOfCode.Year2015;

public class Day23Idiomatic : BaseSolver
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
            //presunuti jump sem protoze ma jen offset a nesaha na regist
            if (instruction.OpCode == "jmp")
            {
                rip += instruction.Offset;
                continue;
            }

            ref int r = ref (instruction.Register == 'a' ? ref a : ref b);
            switch (instruction.OpCode)
            {
                case "hlf":
                    r /= 2;
                    rip++;
                    break;
                case "tpl":
                    r *= 3;
                    rip++;
                    break;
                case "inc":
                    r += 1;
                    rip++;
                    break;
                case "jie": rip += (r % 2 == 0) ? instruction.Offset : 1; break;
                case "jio": rip += (r == 1) ? instruction.Offset : 1; break;
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
/* IDIOMATIC
pridani             ref r = (instruction.Register == 'a' ? ref a : ref b);
misto if regeister a else b v kazdym switch
a presunuti jmp pred switch aby
*/