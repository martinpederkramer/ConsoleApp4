using DsDbLib.DataAccess;
//https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service
//https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service-with-installer?tabs=wix
namespace ConsoleTest4;
public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine((int)DateTime.Now.DayOfWeek);
    }
}
