using DsDbLib.Models;

namespace DsDbLib.DataAccess
{
    public interface IDocuObject
    {
        List<Data> GetData(DsModule module);
        DsModule GetDsModuleTree(string machineName);
        List<string> GetMachineNames();
        List<Message> GetMessages(DsModule module);
        List<Parameter> GetParameters(DsModule module);
        List<Process> GetProcess(DsModule module);
        List<Tag> GetTags(DsModule module);
    }
}