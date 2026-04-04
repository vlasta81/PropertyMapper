using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace PropertyMapper.Benchmarks;

internal static class Program
{
    private static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(
            args,
            new BenchmarkConfig().WithOptions(ConfigOptions.JoinSummary));
    }
}
