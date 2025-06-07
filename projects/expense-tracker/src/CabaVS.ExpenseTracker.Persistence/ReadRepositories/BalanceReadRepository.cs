using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.ReadRepositories;

internal sealed class BalanceReadRepository(ISqlConnectionFactory sqlConnectionFactory) : IBalanceReadRepository
{
    public async Task<BalanceModel[]> GetAllBalancesAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT DISTINCT [b].[Id], [b].[Name], [b].[Amount], [c].[Id], [c].[Name], [c].[Code], [c].[Symbol]
            	FROM [dbo].[Balances] AS [b]
            INNER JOIN [dbo].[Currencies] AS [c]
            	ON [c].[Id] = [b].[CurrencyId]
            WHERE [b].[WorkspaceId] = @workspaceId
            ORDER BY [b].[Name]
            """;

        IEnumerable<BalanceModel> balances = await connection.QueryAsync<BalanceModel, CurrencyModel, BalanceModel>(
            sql,
            (b, c) => b with { Currency = c },
            new { workspaceId },
            splitOn: "Id");
        return [.. balances];
    }

    public async Task<BalanceModel?> GetBalanceByIdAsync(Guid balanceId, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT TOP(1) [b].[Id], [b].[Name], [b].[Amount], [c].[Id], [c].[Name], [c].[Code], [c].[Symbol]
            	FROM [dbo].[Balances] AS [b]
            INNER JOIN [dbo].[Currencies] AS [c]
            	ON [c].[Id] = [b].[CurrencyId]
            WHERE [b].[Id] = @balanceId AND [b].[WorkspaceId] = @workspaceId
            ORDER BY [b].[Name]
            """;

        IEnumerable<BalanceModel> balances = await connection.QueryAsync<BalanceModel, CurrencyModel, BalanceModel>(
            sql,
            (b, c) => b with { Currency = c },
            new { balanceId, workspaceId },
            splitOn: "Id");
        return balances.SingleOrDefault();
    }
}
