using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.ReadRepositories;

internal sealed class CurrencyReadRepository(ISqlConnectionFactory sqlConnectionFactory) : ICurrencyReadRepository
{
    public async Task<CurrencyModel[]> GetAllCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT DISTINCT [c].[Id], [c].[Name], [c].[Code], [c].[Symbol]
            	FROM [dbo].[Currencies] AS [c]
            ORDER BY [c].[Name]
            """;
        
        IEnumerable<CurrencyModel> currencies = await connection.QueryAsync<CurrencyModel>(sql);
        return [.. currencies];
    }

    public async Task<CurrencyModel?> GetCurrencyByIdAsync(Guid currencyId, CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT [c].[Id], [c].[Name], [c].[Code], [c].[Symbol]
                FROM [dbo].[Currencies] AS [c]
            WHERE [c].[Id] = @currencyId
            """;
        
        CurrencyModel? currency = await connection.QueryFirstOrDefaultAsync<CurrencyModel>(sql, new { currencyId });
        return currency;
    }
}
