using DsDbLib.DataAccess;
using DsDbLib.Models;
using DsDbLib.Helpers;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace ConsoleTest4;

public class Program
{
    static void Main(string[] args)
    {

    }
}
public class OeeBatch
{
    public TimeSpan CycleTime { get; set; }
    public int BatchId { get; set; }
    public string? BatchName { get; set; }
    public int State { get; set; }
    public int RecipeId { get; set; }
    public string? RecipeName { get; set; }
}
public class OeeRun
{
    public int Status { get; set; }
}

public class OeeCount
{
    public int[]? Counters { get; set; }
}
public class OeeAutoEvent : OeeEvent { }
public class OeeManualEvent : OeeEvent { }
public class OeeEvent
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public bool Status { get; set; }
}
