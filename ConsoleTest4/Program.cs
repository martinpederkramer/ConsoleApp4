using System.Xml.Linq;

namespace ConsoleTest4;

public class Program
{
    static void Main(string[] args)
    {

    }
    static void TestRecursive()
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
            PrintRecursive(element);
        }
    }

    static int PrintAllElements(XElement root)
    {
        int count = 0;
        Stack<XElement> stack = new Stack<XElement>();
        stack.Push(root);
        XElement current;
        while (stack.Count > 0)
        {
            current = stack.Pop();
            count++;
            Console.WriteLine(current.Name);
            foreach (var attribute in current.Attributes())
            {
                Console.WriteLine("\t" + attribute.Name + "=" + attribute.Value);
            }
            foreach (XElement element in current.Elements())
                stack.Push(element);
        }
        return count;
    }
}
public class Person
{
    public string Name { get; set; }
    public Person()
    {
        
    }
}