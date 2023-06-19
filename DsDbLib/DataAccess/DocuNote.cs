using Dapper;
using DsDbLib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DsDbLib.DataAccess;

public class DocuNote : DocuCommon
{
    public List<string> GetMachineNames()
    {
        using (var con = GetConnection())
        {
            var sql = "SELECT CellIdentifier FROM Cell";

            return con.Query<string>(sql).OrderBy(x => x).ToList();
        }
    }
    public DsModule GetDsModuleTree(string machineName)
    {
        var root = new DsModule()
        {
            Name = machineName,
            Type = ModuleType.Cell
        };
        using (var con = GetConnection())
        {
            con.Open();

            var sql = @"SELECT Id, Name FROM Cell WHERE CellIdentifier = @MachineNo";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@MachineNo", machineName);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                root.Id = reader.GetGuid(0).ToString();
                root.Description = reader.GetString(1);
            }
            reader.Close();
            sql = @"SELECT Id, UnitIdentifier, Name FROM Unit WHERE CellId = @Id ORDER BY UnitIdentifier";
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Guid.Parse(root.Id!));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var un = new DsModule();
                un.Type = ModuleType.Unit;
                un.Id = reader.GetGuid(0).ToString();
                un.Name = reader.GetString(1);
                un.Description = reader.GetString(2);
                root.Childs.Add(un);
            }
            reader.Close() ;
            sql = @"SELECT Id, ModuleIdentifier, Name FROM Module WHERE UnitId = @Id ORDER BY ModuleIdentifier";
            foreach (var child in root.Childs)
            {
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Id", Guid.Parse(child.Id!));
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var em = new DsModule();
                    em.Type = ModuleType.Em;
                    em.Id = reader.GetGuid(0).ToString();
                    em.Name = reader.GetString(1);
                    em.Description = reader.GetString(2);
                    child.Childs.Add(em);
                }
                reader.Close();
            }

            reader.Close();
        }
        return root;
    }

    public List<Tag> GetTags(DsModule module)
    {
        var output = new List<Tag>();
        var sql =
@"SELECT DSModule
,DSTag
,DSComponent
,DSFunctionUK
,DSManufactor
,DSTypeNo
,DSHomePos
,DSIoType
,DSIoTermNo
,DSIoAdress
,DSControlModul
,DSCpuTAG
,DSSpare1
,DSSpare2
,DSSpare3
,DSSpare4
,DSSpare5
,DSSpare6
,DSSpare7
,DSSpare8
,DSSpare9
,DSSpare10
FROM ListTags WHERE ModuleId = @Id";
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Guid.Parse(module.Id!));
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
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
                if (!reader.IsDBNull(11))
                    tag.CpuTag = reader.GetString(11);
                if (!reader.IsDBNull(12))
                    tag.Spare1 = reader.GetString(12);
                if (!reader.IsDBNull(13))
                    tag.Spare2 = reader.GetString(13);
                if (!reader.IsDBNull(14))
                    tag.Spare3 = reader.GetString(14);
                if (!reader.IsDBNull(15))
                    tag.Spare4 = reader.GetString(15);
                if (!reader.IsDBNull(16))
                    tag.Spare5 = reader.GetString(16);
                if (!reader.IsDBNull(17))
                    tag.Spare6 = reader.GetString(17);
                if (!reader.IsDBNull(18))
                    tag.Spare7 = reader.GetString(18);
                if (!reader.IsDBNull(19))
                    tag.Spare8 = reader.GetString(19);
                if (!reader.IsDBNull(20))
                    tag.Spare9 = reader.GetString(20);
                if (!reader.IsDBNull(21))
                    tag.Spare10 = reader.GetString(21);

                if (! String.IsNullOrEmpty(tag.FullName))
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
@"SELECT DSName
,ModuleId
,DSGroup
,DSAlarmtextDK
,DSAlarmtextUK
,DSAlarmtextNA
,DSEmNumber
,DSCmNumber
,DSIdNumber
,DSDelay
,DSTrigger
,DSTested
,DSCPU
,DSTroubleShoot
,DSSpare1
,DSSpare2
,DSSpare3
,DSSpare4
,DSSpare5
,DSSpare6
,DSSpare7
,DSSpare8
,DSSpare9
,DSSpare10
FROM ListMessage WHERE ModuleId = @Id";
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Guid.Parse(module.Id!));
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
                if (!reader.IsDBNull(11))
                    msg.CPU = reader.GetString(11);
                if (!reader.IsDBNull(12))
                    msg.TroubleShoot = reader.GetString(12);
                if (! reader.IsDBNull(13))
                    msg.Spare1 = reader.GetString(13);
                if (!reader.IsDBNull(14))
                    msg.Spare2 = reader.GetString(14);
                if (!reader.IsDBNull(15))
                    msg.Spare3 = reader.GetString(15);
                if (!reader.IsDBNull(16))
                    msg.Spare4 = reader.GetString(16);
                if (!reader.IsDBNull(17))
                    msg.Spare5 = reader.GetString(17);
                if (!reader.IsDBNull(18))
                    msg.Spare6 = reader.GetString(18);
                if (!reader.IsDBNull(19))
                    msg.Spare7 = reader.GetString(19);
                if (!reader.IsDBNull(20))
                    msg.Spare8 = reader.GetString(20);
                if (!reader.IsDBNull(21))
                    msg.Spare9 = reader.GetString(21);
                if (!reader.IsDBNull(22))
                    msg.Spare10 = reader.GetString(22);
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
@"SELECT DSName
,DSFunctionDK
,DSFunctionUK
,DSFunctionNA
,DSType
,DSGroup
,DSUnit
,DSMin
,DSMax
,DSSecurityLevel
,DSDecimals
,DSTested
,DSInitialValue
,DSKeyPadCaption
,DSSwState
,DSAuditTrail
,DSSpare1
,DSSpare2
,DSSpare3
,DSSpare4
,DSSpare5
,DSSpare6
,DSSpare7
,DSSpare8
,DSSpare9
,DSSpare10
FROM ListPars WHERE ModuleId = @Id";
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Guid.Parse(module.Id!));
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
                if (!reader.IsDBNull(12))
                    par.InitialValue = reader.GetString(12);
                if (!reader.IsDBNull(13))
                    par.KeyPadCaption = reader.GetString(13);
                if (!reader.IsDBNull(14))
                    par.SwState = reader.GetString(14);
                if (!reader.IsDBNull(15))
                    par.AuditTrail = reader.GetString(15);
                if (!reader.IsDBNull(16))
                    par.Spare1 = reader.GetString(16);
                if (!reader.IsDBNull(17))
                    par.Spare2 = reader.GetString(17);
                if (!reader.IsDBNull(18))
                    par.Spare3 = reader.GetString(18);
                if (!reader.IsDBNull(19))
                    par.Spare4 = reader.GetString(19);
                if (!reader.IsDBNull(20))
                    par.Spare5 = reader.GetString(20);
                if (!reader.IsDBNull(21))
                    par.Spare6 = reader.GetString(21);
                if (!reader.IsDBNull(22))
                    par.Spare7 = reader.GetString(22);
                if (!reader.IsDBNull(23))
                    par.Spare8 = reader.GetString(23);
                if (!reader.IsDBNull(24))
                    par.Spare9 = reader.GetString(24);
                if (!reader.IsDBNull(25))
                    par.Spare10 = reader.GetString(25);
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
@"SELECT DSName
,DSFunctionDK
,DSFunctionUK
,DSFunctionNA
,DSType
,DSUnit
,DSTested
,DSSpare1
,DSSpare2
,DSSpare3
,DSSpare4
,DSSpare5
,DSSpare6
,DSSpare7
,DSSpare8
,DSSpare9
,DSSpare10
FROM ListProcess WHERE ModuleId = @Id";
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Guid.Parse(module.Id!));
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
                if (!reader.IsDBNull(7))
                    process.Spare1 = reader.GetString(7);
                if (!reader.IsDBNull(8))
                    process.Spare2 = reader.GetString(8);
                if (!reader.IsDBNull(9))
                    process.Spare3 = reader.GetString(9);
                if (!reader.IsDBNull(10))
                    process.Spare4 = reader.GetString(10);
                if (!reader.IsDBNull(11))
                    process.Spare5 = reader.GetString(11);
                if (!reader.IsDBNull(12))
                    process.Spare6 = reader.GetString(12);
                if (!reader.IsDBNull(13))
                    process.Spare7 = reader.GetString(13);
                if (!reader.IsDBNull(14))
                    process.Spare8 = reader.GetString(14);
                if (!reader.IsDBNull(15))
                    process.Spare9 = reader.GetString(15);
                if (!reader.IsDBNull(16))
                    process.Spare10 = reader.GetString(16);
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
@"SELECT DSName
,DSDescription
,DSType
,DSGroup
,DSTested
,DSInitValue
,DSSpare1
,DSSpare2
,DSSpare3
,DSSpare4
,DSSpare5
,DSSpare6
,DSSpare7
,DSSpare8
,DSSpare9
,DSSpare10
FROM ListData WHERE ModuleId = @Id";
        using (var con = GetConnection())
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Guid.Parse(module.Id!));
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
                if (!reader.IsDBNull(5))
                    data.InitialValue = reader.GetString(5);
                if (!reader.IsDBNull(6))
                    data.Spare1 = reader.GetString(6);
                if (!reader.IsDBNull(7))
                    data.Spare2 = reader.GetString(7);
                if (!reader.IsDBNull(8))
                    data.Spare3 = reader.GetString(8);
                if (!reader.IsDBNull(9))
                    data.Spare4 = reader.GetString(9);
                if (!reader.IsDBNull(10))
                    data.Spare5 = reader.GetString(10);
                if (!reader.IsDBNull(11))
                    data.Spare6 = reader.GetString(11);
                if (!reader.IsDBNull(12))
                    data.Spare7 = reader.GetString(12);
                if (!reader.IsDBNull(13))
                    data.Spare8 = reader.GetString(13);
                if (!reader.IsDBNull(14))
                    data.Spare9 = reader.GetString(14);
                if (!reader.IsDBNull(15))
                    data.Spare10 = reader.GetString(15);

                if (!String.IsNullOrEmpty(data.Name))
                    output.Add(data);
            }
            reader.Close();
        }
        return output;
    }
}
