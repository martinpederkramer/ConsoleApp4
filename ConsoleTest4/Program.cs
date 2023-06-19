using DsDbLib.DataAccess;
using DsDbLib.Models;
using DsDbLib.Helpers;
using System.Reflection.Emit;

namespace ConsoleTest4;
class Program
{
    const string basePath = @"F:\Arkiv";
    static void Main(string[] args)
    {
        TestDb();
        //TestDir();
    }
    static void TestDir()
    {
        var machinePath = DirectoryHelper.GetMachineDirectory(basePath, "M-001361");
        Console.WriteLine(machinePath);
        var dsPath = DirectoryHelper.GetDsDirectory(machinePath);
        Console.WriteLine(dsPath);
        var un01Path = DirectoryHelper.GetModuleDirectory(dsPath, "UN01");
        Console.WriteLine(un01Path);
        var un01em02Path = DirectoryHelper.GetModuleDirectory(un01Path, "EM03");
        Console.WriteLine(un01em02Path);
    }
    static void TestDb()
    {
        IDocuObject db = new DocuBase();
        var names = db.GetMachineNames();
        DsModule moduleTree;
        //moduleTree = db.GetDsModuleTree("M-001280");

        moduleTree = db.GetDsModuleTree("M-001260");//3PSH Docubase
        //moduleTree = db.GetDsModuleTree("M-001361");//Docubase

        foreach (var module in moduleTree.GetDsModuleList().Where(x => x.Type == ModuleType.Em))
        {
            foreach (var msg in db.GetParameters(module))
            {
                Console.WriteLine(msg);
            }
        }
    }
}
