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
        TestDb();
    }
    static void TestDir()
    {
        var machinePath = DirectoryHelper.GetMachineDirectory(basePath, "M-001361");
        Console.WriteLine(machinePath);
        var dsPath = DirectoryHelper.GetDsDirectory(machinePath);
        Console.WriteLine(dsPath);
        var un01Path = DirectoryHelper.GetModuleDirectory(dsPath, "UN01");
        Console.WriteLine(un01Path);
        var un01em02Path = DirectoryHelper.GetModuleDirectory(un01Path, "EM02");
        Console.WriteLine(un01em02Path);
    }
    static void TestDb()
    {
        var db = new DocuNote();
        var names = db.GetMachineNames();
        var moduleTree = db.GetDsModuleTree("M-001280");
        //var moduleTree = db.GetDsModuleTree("M-001361");
        foreach (var module in moduleTree.GetDsModuleList().Where(x => x.Type == ModuleType.Em))
        {
            foreach (var msg in db.GetData(module))
            {
                Console.WriteLine(msg);
            }
        }
    }
}
