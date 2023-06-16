using DsDbLib.DataAccess;
using DsDbLib.Models;
using System.Reflection.Emit;

namespace ConsoleTest4;
class Program
{
    static void Main(string[] args)
    {
        var db = new DocuNote();
        var names = db.GetMachineNames();
        var moduleTree = db.GetModuleTree("M-001280");
        //var moduleTree = db.GetModuleTree("M-001361");
        foreach (var module in moduleTree.GetModules().Where(x => x.Type == ModuleType.Em))
        {
            foreach (var msg in db.GetTags(module))
            {
                Console.WriteLine(msg);
            }
        }
    }
}
