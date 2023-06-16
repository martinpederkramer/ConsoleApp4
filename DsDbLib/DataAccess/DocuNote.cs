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
            con.Close();
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
            con.Close();
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
                msg.Group = reader.GetGuid(1).ToString();
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
            con.Close();
        }
        return output;
    }
}
