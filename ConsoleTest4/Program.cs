using DsDbLib.DataAccess;
using DsDbLib.Models;
using DsDbLib.Helpers;
using System.Reflection.Emit;

namespace ConsoleTest4;

public class Program
{
    static void Main(string[] args)
    {
        TestDb();
        //TestDir();
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
        }
        return count;
    }
}
