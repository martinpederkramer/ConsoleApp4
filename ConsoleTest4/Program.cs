using DsDbLib.DataAccess;

namespace ConsoleTest4;
public class Program
{
    static void Main(string[] args)
    {
        IDocuObject docuObject = new DocuNote();
        foreach (string machine in docuObject.GetMachineNames())
        {
            Console.WriteLine(machine);
        }
    }
}
