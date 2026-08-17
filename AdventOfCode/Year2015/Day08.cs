using System.Text;

namespace AdventOfCode.Year2015;

public class Day08 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 8;

    public override string SolvePart1(string[] input)
    {
        int sumCode = 0;
        int sumData = 0;
        foreach (var str in input)
        {
            // code > data
            int code = str.Length;
            int data = 0;

            string inner = str.Substring(1, str.Length - 2);

            for (int i = 0; i < inner.Length;)
            {
                char c = inner[i];

                switch (c)
                {
                    case '\\':
                    {
                        char cPlus1 = inner[i + 1];

                        switch (cPlus1)
                        {
                            case '\\':
                            {
                                data += 1;
                                i += 2;
                                break;
                            }
                            case '"':
                            {
                                data += 1;
                                i += 2;
                                break;
                            }
                            case 'x':
                            {
                                var hex = Convert.FromHexString(new[] { inner[i + 2], inner[i + 3] });
                                var ascii = Encoding.ASCII.GetString(hex);

                                data += 1;
                                i += 4;
                                break;
                            }
                        }

                        break;
                    }
                    default:
                    {
                        data += 1;
                        i++;
                        break;
                    }
                }
            }

            sumCode += code;
            sumData += data;

            //Console.WriteLine($"{code} {data}");
        }


        return (sumCode - sumData).ToString();
    }

    public override string SolvePart2(string[] input)
    {
        return "Not implemented yet";
    }
}