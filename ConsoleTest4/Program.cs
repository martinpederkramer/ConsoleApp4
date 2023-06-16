using DsDbLib.DataAccess;
using DsDbLib.Models;
using System.Reflection.Emit;

namespace ConsoleTest4;
class Program
{
    static void Main(string[] args)
    {
        var msv = GetDirs(@"F:\Arkiv\M-001361");
        for (int i = 0; i < msv.GetLength(0); i++)
        {
            Console.WriteLine($"{msv[i,0]}\t{msv[i, 1]}");
        }
    }
    private static string[,]? GetDirs(string path)
    {
        var dirs = Directory.GetDirectories(path);
        if (dirs == null)
            return null;

        var dirArray = new string[dirs.Length, 2];
        for (int i = 0; i < dirs.Length; i++)
        {
            dirArray[i,0] = dirs[i];
            var dirSplitted = dirs[i].Split('\\');
            dirArray[i,1] = dirSplitted[dirSplitted.Length - 1];
        }
        return dirArray;
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
