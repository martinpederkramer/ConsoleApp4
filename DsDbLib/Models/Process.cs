using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsDbLib.Models;

public class Process : DsBase
{
    public string? Name { get; set; }
    public string? FunctionDK { get; set; }
    public string? FunctionUK { get; set; }
    public string? FunctionNA { get; set; }
    public string? Type { get; set; }
    public string? Unit { get; set; }
    public string? Tested { get; set; }
    public override string? ToString()
    {
        return Name;
    }
}
