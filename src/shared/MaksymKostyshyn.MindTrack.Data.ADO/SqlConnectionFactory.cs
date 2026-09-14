using Microsoft.Data.SqlClient;

namespace MaksymKostyshyn.MindTrack.Data.ADO;

public sealed class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    private readonly string _connectionString = connectionString;

    public SqlConnection CreateConnection() => new(_connectionString);
}
