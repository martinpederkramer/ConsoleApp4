using DsDbLib.DataAccess;

namespace ConsoleTest4;
public class Program
{
    static void Main(string[] args)
    {
        string screenFile = @"D:\temp\xml\HMI_RT_1\screens\un01_em01_Par.xml";

        XDocument doc = XDocument.Load(screenFile);
        XElement root = doc.Root!;
        XElement screen = root.Element("Hmi.Screen.Screen")!;
        PrintAllElements(root);
    }
    static void PrintRecursive(XElement root)
    {
        Console.WriteLine(root.Name);
        foreach (XElement element in root.Elements())
        {
            Console.WriteLine(machine);
        }
    }
}
public class Person
{
    public string Name { get; set; }
    public Person()
    {
        
    }
}