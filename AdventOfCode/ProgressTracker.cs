using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode;

/// <summary>
/// A static thread-safe tracker for long-running operations. 
/// Updates are printed periodically by a background thread 
/// to avoid spamming the test runner output and causing OutOfMemory issues.
/// </summary>
public static class ProgressTracker
{
    private static long _counter;
    private static CancellationTokenSource? _cts;
    private static Task? _worker;
    private static string _message = "Processed: ";
    private static TimeSpan _updateInterval = TimeSpan.FromMilliseconds(1000);

    public static void Start(string message = "Processed: ", int updateIntervalMs = 1000)
    {
        Stop(); // ensure any previous is stopped
        
        _message = message;
        _updateInterval = TimeSpan.FromMilliseconds(updateIntervalMs);
        Interlocked.Exchange(ref _counter, 0); // Reset counter

        _cts = new CancellationTokenSource();
        _worker = Task.Run(() => MonitorLoop(_cts.Token));
    }

    public static void Increment()
    {
        Interlocked.Increment(ref _counter);
    }

    public static void Add(long value)
    {
        Interlocked.Add(ref _counter, value);
    }

    public static long CurrentValue => Interlocked.Read(ref _counter);

    private static async Task MonitorLoop(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(_updateInterval, token);
                long current = Interlocked.Read(ref _counter);
                
                // TestContext.Progress writes immediately to the runner,
                // without buffering until the end of the test.
                TestContext.Progress.WriteLine($"{_message}{current}");
            }
        }
        catch (TaskCanceledException)
        {
            // Expected when the delay is cancelled during Stop
        }
    }

    public static void Stop()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            try
            {
                _worker?.Wait();
            }
            catch (AggregateException e) when (e.InnerException is TaskCanceledException)
            {
                // Expected
            }
            _cts.Dispose();
            _cts = null;
            
            TestContext.Progress.WriteLine($"{_message}{Interlocked.Read(ref _counter)} (Finished)");
        }
    }
}
