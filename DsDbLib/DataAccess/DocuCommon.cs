using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsDbLib.DataAccess;

public abstract class DocuCommon
{
    private const string ConnectionString = @"Data Source=SVM-AXSQL;Database=DS_DB;User ID=ExcelMacros;Password=ExcelMacros;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    protected SqlConnection GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }
    protected int ParseInt(string? str)
    {
        int.TryParse(str, out int i);
        return i;
    }
}
