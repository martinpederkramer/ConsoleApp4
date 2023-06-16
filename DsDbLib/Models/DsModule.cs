using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsDbLib.Models;

public class DsModule
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ModuleType Type { get; set; }
    public DsModule? Parent { get; set; }
    public List<DsModule> Childs { get; set; } = new List<DsModule>();
    public override string? ToString()
    {
        return Name;
    }
    public List<DsModule> GetModules()
    {
        List<DsModule> output = new();
        output.Add(this);
        GetModuleRecursive(this, output);

        return output;
    }
    private void GetModuleRecursive(DsModule module, List<DsModule> output)
    {
        foreach (var item in module.Childs)
        {
            output.Add(item);
            GetModuleRecursive(item, output);
        }
    }
}
public enum ModuleType
{
    Cell,
    Unit,
    Em
}