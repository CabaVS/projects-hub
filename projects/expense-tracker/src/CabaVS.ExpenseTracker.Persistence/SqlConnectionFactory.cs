using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class SqlConnectionFactory(IServiceProvider serviceProvider) : ISqlConnectionFactory
{
    public SqlConnection CreateConnection() => serviceProvider.GetRequiredService<SqlConnection>();
}
