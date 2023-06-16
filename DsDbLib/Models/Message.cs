using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsDbLib.Models;

public class Message : DsBase
{
    public string? Name { get; set; }
    public string? Group { get; set; }
    public string? AlarmtextDK { get; set; }
    public string? AlarmtextUK { get; set; }
    public string? AlarmtextNA { get; set; }
    public int EmNumber { get; set; }
    public int CmNumber { get; set; }
    public int IdNumber { get; set; }
    public int Delay { get; set; }
    public string? Trigger { get; set; }
    public string? Tested { get; set; }
    public string? CPU { get; set; }
    public string? TroubleShoot { get; set; }
    public override string ToString()
    {
        return $"{Name} : {AlarmtextUK}";
    }
}
