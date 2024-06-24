using DsDbLib.DataAccess;
using DsDbLib.Models;
using DsDbLib.Helpers;
using Pro = System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.VisualBasic;
using System.Text;
using System.IO.Compression;
using System.Diagnostics;
using System.Xml.Linq;

namespace ConsoleTest4;
class Program
{
    const string basePath = @"F:\Arkiv";
    static void Main(string[] args)
    {
        //TestProcess();
        //TestDb();
        //TestDir();
        //HandiParkDiff();

        Stopwatch sw = new Stopwatch();
        sw.Start();
        int i = 0;
        string s;
        using (FileStream zipToOpen = new FileStream(@"V:\STD-Baseprograms\ParameterValidator\data\M-001361\M-001361.zip", FileMode.Open))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    using (StreamReader sr = new StreamReader(entry.Open()))
                    {
                        s = sr.ReadToEnd();
                        Console.WriteLine(entry.FullName);
                        //XDocument xDocument = XDocument.Parse(s);
                    }
                }
            }
        }
        Console.WriteLine(sw.ElapsedMilliseconds);
        Console.WriteLine(i);
    }
    static void HandiParkDiff()
    {
        DateTime oprettelse = DateTime.Parse("2024-03-08");

        TimeSpan diff = DateTime.Now - oprettelse;

        Console.WriteLine(diff.Days);
        Console.WriteLine(diff.Days / 7d);
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
    static void TestProcess()
    {
        foreach (var item in Pro.Process.GetProcesses().Where(x => x.ProcessName.StartsWith("Con")))
        {
            Console.WriteLine(item.ProcessName);
        }
    }
}
