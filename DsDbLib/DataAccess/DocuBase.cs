using DsDbLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace DsDbLib.DataAccess;

public class DocuBase : DocuCommon
{
    public List<string> GetMachineNames()
    {
        using (var con = GetConnection())
        {
            var sql = "SELECT DISTINCT DSmachineNo FROM Tags";

            return con.Query<string>(sql).OrderBy( x => x).ToList();
        }
    }
    public DsModule GetModuleTree(string machineName)
    {
        string[] dsIds;
        using (var con = GetConnection())
        {
            var sql = @"SELECT DISTINCT DSID FROM Tags WHERE DSmachineNo = @MachineNo";
            var par = new { MachineNo = machineName };
            dsIds = con.Query<string>(sql, par).Where(x => x.Length == 15).ToArray();
        }

        var root = new DsModule()
        {
            Id = $"M{machineName.Substring(2, 6)}",
            Name = machineName,
            Type = ModuleType.Cell
        };

        foreach (var unId in dsIds.Select(x => x.Substring(7, 4)).Distinct())
        {
            var un = new DsModule()
            {
                Id = root.Id + unId,
                Name = unId,
                Parent = root,
                Type = ModuleType.Unit
            };
            root.Childs.Add(un);

            foreach (var emId in dsIds.Where(x => x.Substring(7, 4) == un.Name).Distinct())
            {
                var em = new DsModule()
                {
                    Id = emId,
                    Name = emId.Substring(11, 4),
                    Parent = un,
                    Type = ModuleType.Em
                };
                un.Childs.Add(em);
            }
        }
        return root;
    }
    public List<Tag> GetTags(DsModule module)
    {
        return new List<Tag>();
    }
    public List<Message> GetMessages(DsModule module)
    {
        var output = new List<Message>();
        var sql =
@"SELECT DSname
,DSgroup
,DSalarmtextDK
,DSalarmtextUK
,DSalarmtextNA
,DSemNumber
,DScmNumber
,DSidNumber
,DSdelay
,DStrigger
,DStested
FROM Message WHERE DSID = @Id";
        int i;
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", module.Id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Message msg = new Message();
                msg.Parent = module;
                msg.Name = reader.GetString(0);
                msg.Group = reader.GetString(1);
                msg.AlarmtextDK = reader.GetString(2);
                msg.AlarmtextUK = reader.GetString(3);
                msg.AlarmtextNA = reader.GetString(4);
                msg.EmNumber = ParseInt(reader.GetString(5));
                msg.CmNumber = ParseInt(reader.GetString(6));
                msg.IdNumber = ParseInt(reader.GetString(7));
                msg.Delay = ParseInt(reader.GetString(8));
                msg.Trigger = reader.GetString(9);
                msg.Tested = reader.GetString(10);
                output.Add(msg);
            }
            reader.Close();
            con.Close();
        }
        return output;
    }
    public List<Parameter> GetParameters(DsModule module)
    {
        return new List<Parameter>();
    }
    public List<Data> GetDatas(DsModule module)
    {
        return new List<Data>();
    }
    public List<Process> GetProcesses(DsModule module)
    {
        return new List<Process>();
    }
}
