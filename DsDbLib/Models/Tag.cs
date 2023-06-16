using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsDbLib.Models;

public class Tag : DsBase
{
    public string? Module { get; set; }
    public string? Name { get; set; }
    public string? Component { get; set; }
    public string? Function { get; set; }
    public string? Manufactor { get; set; }
    public string? TypeNo { get; set; }
    public string? HomePosition { get; set; }
    public string? IoType { get; set; }
    public string? IoTerminalNo { get; set; }
    public string? IoAddress { get; set; }
    public string? ControlModule { get; set; }
    public string? CpuTag { get; set; }
    public string? FullName
    {
        get
        {
            if (String.IsNullOrEmpty(Name))
                return Module;
            return $"{Module}-{Name}";
        }
    }
    public override string? ToString()
    {
        return FullName;
    }
}
