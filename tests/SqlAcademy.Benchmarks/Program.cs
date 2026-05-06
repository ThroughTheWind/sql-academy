using BenchmarkDotNet.Running;

namespace SqlAcademy.Benchmarks;

internal static class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<TrackingModeBenchmarks>();
    }
}
