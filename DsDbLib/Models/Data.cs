using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsDbLib.Models;

public class Data : DsBase
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public string? Group { get; set; }
    public string? Tested { get; set; }
    public string? InitialValue { get; set; }
    public override string? ToString()
    {
        return Name;
    }
}
