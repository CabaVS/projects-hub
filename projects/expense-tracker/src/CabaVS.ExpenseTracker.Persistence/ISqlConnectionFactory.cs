using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
