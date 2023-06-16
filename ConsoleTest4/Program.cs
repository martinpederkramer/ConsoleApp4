using DsDbLib.DataAccess;
using DsDbLib.Models;
using DsDbLib.Helpers;
using System.Reflection.Emit;

namespace ConsoleTest4;
class Program
{
    const string basePath = @"D:\arkiv";
    static void Main(string[] args)
    {
        var machinePath = DirectoryHelper.GetMachineDir(basePath, "M-001361");
        Console.WriteLine(machinePath);
        var dsDir = DirectoryHelper.GetDsDir(machinePath);
        Console.WriteLine(dsDir);
        var un01Dir = DirectoryHelper.GetModuleDir(dsDir, "UN01");
        Console.WriteLine(un01Dir);
        var un01em02dir = DirectoryHelper.GetModuleDir(un01Dir, "EM02");
        Console.WriteLine(un01em02dir);
    }

    static void TestDb()
    {
        var db = new DocuBase();
        var names = db.GetMachineNames();
        //var moduleTree = db.GetModuleTree("M-001280");
        var moduleTree = db.GetModuleTree("M-001361");
        foreach (var module in moduleTree.GetModules().Where(x => x.Type == ModuleType.Em))
        {
            foreach (var msg in db.GetTags(module))
            {
                Console.WriteLine(msg);
            }
        }
    }
}
