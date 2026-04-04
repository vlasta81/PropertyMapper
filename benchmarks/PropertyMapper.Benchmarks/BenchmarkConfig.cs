using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;

namespace PropertyMapper.Benchmarks;

/// <summary>
/// Shared BenchmarkDotNet configuration passed as the global config in <c>Program.cs</c>.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Medium-run job — 3 launches × 5 warm-up iterations + 15 measurement iterations.</item>
/// <item>Server GC — avoids Workstation GC pauses skewing measurements.</item>
/// <item>Memory diagnoser — reports Gen0/Gen1/Gen2 GC collections and allocated bytes.</item>
/// <item>Min/Max columns — expose best-case and worst-case timing.</item>
/// <item>Ratio trend — shows speed relative to the baseline method.</item>
/// <item>Exporters — GitHub Markdown (explicit); HTML, CSV, and compressed JSON come from the default configuration.</item>
/// </list>
/// </remarks>
public sealed class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddLogger(ConsoleLogger.Default);
        AddColumnProvider(DefaultColumnProviders.Instance);

        AddJob(Job.MediumRun
            .WithWarmupCount(5)
            .WithIterationCount(15)
            .WithLaunchCount(2)
            .WithGcServer(true)
            .WithId(".NET 10"));

        AddDiagnoser(MemoryDiagnoser.Default);
        AddColumn(StatisticColumn.Min);
        AddColumn(StatisticColumn.Max);
        AddColumn(RankColumn.Arabic);

        AddExporter(MarkdownExporter.GitHub);

        Orderer = new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest);
        SummaryStyle = SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend);
    }
}
