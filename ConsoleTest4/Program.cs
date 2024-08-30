using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DsDbLib.DataAccess;
using System.Xml.Linq;

namespace ConsoleTest4;
public class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<Test>();
    }
}
[MemoryDiagnoser]
public class Test
{
    [Benchmark]
    public void LoopLines()
    {
        int j = 0;
        for (int i = 0; i < 10; i++)
        {
            j+=i;
        }
    }
}