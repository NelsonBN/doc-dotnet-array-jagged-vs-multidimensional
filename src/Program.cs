using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<InstanciateBenchmark>();
BenchmarkRunner.Run<FillBenchmark>();
BenchmarkRunner.Run<AccessBenchmark>();

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class InstanciateBenchmark
{
    [Params(1, 10, 100, 1_000, 10_000)]
    public int Items;

    [Benchmark(Baseline = true)]
    public void Jagger()
    {
        var jagger = new int[Items][];
        for (var i = 0; i < Items; i++)
        {
            jagger[i] = new int[Items];
        }
    }

    [Benchmark]
    public void MultiDimensional()
    {
        var multiDimensional = new int[Items, Items];
    }
}



[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class FillBenchmark
{
    [Params(1, 10, 100, 1_000, 10_000)]
    public int Items;

    private int[][]? _jagger;
    private int[,]? _multiDimensional;

    [GlobalSetup]
    public void Setup()
    {
        _jagger = new int[Items][];

        for (var i = 0; i < Items; i++)
        {
            _jagger[i] = new int[Items];
        }

        _multiDimensional = new int[Items, Items];
    }

    [Benchmark(Baseline = true)]
    public void Jagger()
    {
        for (var i = 0; i < Items; i++)
        {
            for (var j = 0; j < Items; j++)
            {
                _jagger[i][j] = i * j;
            }
        }
    }

    [Benchmark]
    public void MultiDimensional()
    {
        for (var i = 0; i < Items; i++)
        {
            for (var j = 0; j < Items; j++)
            {
                _multiDimensional[i, j] = i * j;
            }
        }
    }
}



[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class AccessBenchmark
{
    [Params(1, 10, 100, 1_000, 10_000)]
    public int Items;

    private int[][]? _jagger;
    private int[,]? _multiDimensional;

    [GlobalSetup]
    public void Setup()
    {
        _jagger = new int[Items][];

        for (var i = 0; i < Items; i++)
        {
            _jagger[i] = new int[Items];
        }

        _multiDimensional = new int[Items, Items];

        for (var i = 0; i < Items; i++)
        {
            for (var j = 0; j < Items; j++)
            {
                _jagger[i][j] = i * j;
                _multiDimensional[i, j] = i * j;
            }
        }
    }

    [Benchmark(Baseline = true)]
    public void Jagger()
    {
        for (var i = 0; i < Items; i++)
        {
            for (var j = 0; j < Items; j++)
            {
                var temp = _jagger[i][j];
            }
        }
    }

    [Benchmark]
    public void MultiDimensional()
    {
        for (var i = 0; i < Items; i++)
        {
            for (var j = 0; j < Items; j++)
            {
                var temp = _multiDimensional[i, j];
            }
        }
    }
}
