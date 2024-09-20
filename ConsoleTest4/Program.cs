using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DsDbLib.DataAccess;
//https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service
//https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service-with-installer?tabs=wix
namespace ConsoleTest4;
public class Program
{
    static void Main(string[] args)
    {
        int[] nums = { 1, 2, 3, 4, 5 };

        foreach (int i in nums.Where(x => x>2))
        {
            Console.WriteLine(i);
        }
    }
}