using DsDbLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using DsDbLib.Helpers;

namespace DsDbLib.DataAccess;

public class DocuBase : DocuCommon, IDocuObject
{
    public List<string> GetMachineNames()
    {
        using (var con = GetConnection())
        {
            var sql = "SELECT DISTINCT DSmachineNo FROM Tags";

            return con.Query<string>(sql).OrderBy(x => x).ToList();
        }
    }
    public DsModule GetDsModuleTree(string machineName)
    {
        string[] dsIds;
        using (var con = GetConnection())
        {
            var sql = @"SELECT DISTINCT DSID FROM Tags WHERE DSmachineNo = @MachineNo";
            var par = new { MachineNo = machineName };
            dsIds = con.Query<string>(sql, par).Where(x => x.Length == 15).ToArray();
        }
        var machinePath = DirectoryHelper.GetMachineDirectory(@"F:\Arkiv", machineName);
        var root = new DsModule()
        {
            Id = $"M{machineName.Substring(2, 6)}",
            Name = machineName,
            Description = machinePath.GetDirectoryName(),
            Type = ModuleType.Cell
        };
        var dsPath = DirectoryHelper.GetDsDirectory(machinePath);
        foreach (var unId in dsIds.Select(x => x.Substring(7, 4)).Distinct())
        {
            var unPath = DirectoryHelper.GetModuleDirectory(dsPath, unId);
            var un = new DsModule()
            {
                Id = root.Id + unId,
                Name = unId,
                Description = unPath.GetDirectoryName(),
                Parent = root,
                Type = ModuleType.Unit
            };
            root.Childs.Add(un);

            foreach (var emId in dsIds.Where(x => x.Substring(7, 4) == un.Name).Distinct())
            {
                var em = new DsModule()
                {
                    Id = emId,
                    Name = emId.Substring(emId.Length - 4),
                    Parent = un,
                    Type = ModuleType.Em
                };
                var emPath = DirectoryHelper.GetModuleDirectory(unPath, em.Name);
                em.Description = emPath.GetDirectoryName();
                un.Childs.Add(em);
            }
        }
        return root;
    }
    public List<Tag> GetTags(DsModule module)
    {
        var output = new List<Tag>();
        var sql =
@"SELECT 
DSmodule
,DStag
,DScomponent
,DSfunctionUK
,DSfunctionDK
,DSfunctionNA
,DSmanufactor
,DStypeNo
,DShomePos
,DSioType
,DSioTermNo
,DSioAdress
,DScontrolModul
FROM Tags WHERE DSID = @Id";
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", module.Id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Tag tag = new Tag();
                tag.Parent = module;
                tag.Module = reader.GetString(0);
                tag.Name = reader.GetString(1);
                tag.Component = reader.GetString(2);
                tag.Function = reader.GetString(3);
                tag.Manufactor = reader.GetString(4);
                tag.TypeNo = reader.GetString(5);
                tag.HomePosition = reader.GetString(6);
                tag.IoType = reader.GetString(7);
                tag.IoTerminalNo = reader.GetString(8);
                tag.IoAddress = reader.GetString(9);
                if (!reader.IsDBNull(10))
                    tag.ControlModule = reader.GetString(10);

                if (!String.IsNullOrEmpty(tag.FullName))
                    output.Add(tag);
            }
            reader.Close();
        }
        return output;
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
        }
        return output;
    }
    public List<Parameter> GetParameters(DsModule module)
    {
        var output = new List<Parameter>();
        var sql =
@"SELECT DSname
,DSfunctionDK
,DSfunctionUK
,DSfunctionNA
,DStype
,DSgroup
,DSunit
,DSmin
,DSmax
,DSsecurityLevel
,DSDecimals
,DStested
FROM Pars WHERE DSID = @Id";
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", module.Id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Parameter par = new Parameter();
                par.Parent = module;
                par.Name = reader.GetString(0);
                par.FunctionDK = reader.GetString(1);
                par.FunctionUK = reader.GetString(2);
                par.FunctionNA = reader.GetString(3);
                par.Type = reader.GetString(4);
                par.Group = reader.GetString(5);
                par.Unit = reader.GetString(6);
                par.Min = reader.GetString(7);
                par.Max = reader.GetString(8);
                if (!reader.IsDBNull(9))
                    par.SecurityLevel = reader.GetString(9);
                if (!reader.IsDBNull(10))
                    par.Decimals = reader.GetString(10);
                if (!reader.IsDBNull(11))
                    par.Tested = reader.GetString(11);

                if (!String.IsNullOrEmpty(par.Name))
                    output.Add(par);
            }
            reader.Close();
        }
        return output;
    }
    public List<Process> GetProcess(DsModule module)
    {
        var output = new List<Process>();
        var sql =
@"SELECT DSname
,DSfunctionDK
,DSfunctionUK
,DSfunctionNA
,DStype
,DSunit
,DStested
FROM Process WHERE DSID = @Id";
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", module.Id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Process process = new Process();
                process.Parent = module;
                process.Name = reader.GetString(0);
                process.FunctionDK = reader.GetString(1);
                process.FunctionUK = reader.GetString(2);
                process.FunctionNA = reader.GetString(3);
                process.Type = reader.GetString(4);
                process.Unit = reader.GetString(5);
                if (!reader.IsDBNull(6))
                    process.Tested = reader.GetString(6);

                if (!String.IsNullOrEmpty(process.Name))
                    output.Add(process);
            }
            reader.Close();
        }
        return output;
    }
    public List<Data> GetData(DsModule module)
    {
        var output = new List<Data>();
        var sql =
@"SELECT DSname
,DSdescription
,DStype
,DSgroup
,DStested
FROM Data WHERE DSID = @Id";
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", module.Id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Data data = new Data();
                data.Parent = module;
                data.Name = reader.GetString(0);
                data.Description = reader.GetString(1);
                data.Type = reader.GetString(2);
                data.Group = reader.GetString(3);
                if (!reader.IsDBNull(4))
                    data.Tested = reader.GetString(4);

                if (!String.IsNullOrEmpty(data.Name))
                    output.Add(data);
            }
            reader.Close();
        }
        return output;
    }
}
