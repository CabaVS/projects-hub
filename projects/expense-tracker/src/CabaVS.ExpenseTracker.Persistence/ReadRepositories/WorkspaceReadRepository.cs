using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.ReadRepositories;

internal sealed class WorkspaceReadRepository(ISqlConnectionFactory sqlConnectionFactory) : IWorkspaceReadRepository
{
    public async Task<WorkspaceSlimModel[]> GetAllWorkspacesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT DISTINCT [w].[Id], [w].[Name]
            	FROM [dbo].[Workspaces] AS [w]
            INNER JOIN [dbo].[WorkspaceMembers] AS [wm]
            	ON [wm].[WorkspaceId] = [w].[Id] AND [wm].[UserId] = @userId
            ORDER BY [w].[Name]
            """;
        
        IEnumerable<WorkspaceSlimModel> workspaces = await connection.QueryAsync<WorkspaceSlimModel>(sql, new { userId });
        return [.. workspaces];
    }

    public async Task<WorkspaceModel?> GetWorkspaceByIdAsync(Guid workspaceId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT [w].[Id], [w].[Name], [wm].[IsAdmin] AS [CurrentUserIsAdmin]
            	FROM [dbo].[Workspaces] AS [w]
            INNER JOIN [dbo].[WorkspaceMembers] AS [wm]
            	ON [wm].[WorkspaceId] = [w].[Id] AND [wm].[UserId] = @userId
            WHERE [w].[Id] = @workspaceId
            """;
        
        WorkspaceModel? workspace = await connection.QueryFirstOrDefaultAsync<WorkspaceModel>(sql, new { workspaceId, userId });
        return workspace;
    }

    public async Task<bool> UserIsAdminOfWorkspaceAsync(Guid workspaceId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT 1 FROM [dbo].[WorkspaceMembers]
            WHERE [WorkspaceId] = @workspaceId
            AND [UserId] = @userId
            AND [IsAdmin] = 1
            """;
        
        var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { workspaceId, userId });
        return result.HasValue;
    }

    public async Task<bool> UserIsMemberOfWorkspaceAsync(Guid workspaceId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT 1 FROM [dbo].[WorkspaceMembers]
            WHERE [WorkspaceId] = @workspaceId
            AND [UserId] = @userId
            """;
        
        var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { workspaceId, userId });
        return result.HasValue;
    }
}
