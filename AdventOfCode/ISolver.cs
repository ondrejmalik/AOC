namespace AdventOfCode;

public interface ISolver
{
    int Year { get; }
    int Day { get; }
    void Solve();
}
