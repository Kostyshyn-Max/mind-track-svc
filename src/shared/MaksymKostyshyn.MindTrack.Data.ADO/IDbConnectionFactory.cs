using Microsoft.Data.SqlClient;

namespace MaksymKostyshyn.MindTrack.Data.ADO;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}
