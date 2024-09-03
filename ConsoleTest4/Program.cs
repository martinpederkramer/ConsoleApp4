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
        var files = GetAllFileNames(@"D:\Temp");

        foreach (var file in files)
        {
            Console.WriteLine(file);
        }
    }
    static List<string> GetAllFileNames(string rootDir)
    {
        List<string> files = new List<string>();
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(rootDir);

        string actDir;
        while (queue.Count > 0)
        {
            actDir = queue.Dequeue();
            foreach (string file in Directory.GetFiles(actDir))
            {
                files.Add(file);
            }
            foreach (var dir in Directory.GetDirectories(actDir))
            {
                queue.Enqueue(dir);
            }
        }
        return files;
    }
}