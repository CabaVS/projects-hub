IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

// Parameters
IResourceBuilder<ParameterResource> sqlUsername = builder.AddParameter("sql-cabavsprojectshub-username", true);
IResourceBuilder<ParameterResource> sqlPassword = builder.AddParameter("sql-cabavsprojectshub-password", true);

// SQL Server
IResourceBuilder<SqlServerServerResource> sql = builder.AddSqlServer("sql-cabavsprojectshub", sqlPassword, port: 1433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

// SQL Databases
IResourceBuilder<SqlServerDatabaseResource> dbKeycloak = sql.AddDatabase("sqldb-keycloak");
IResourceBuilder<SqlServerDatabaseResource> dbExpenseTracker = sql.AddDatabase("sqldb-expensetracker");

// Keycloak
IResourceBuilder<KeycloakResource> keycloak = builder.AddKeycloak("aca-keycloak", 4800)
    .WithEnvironment("KC_DB", "mssql")
    .WithEnvironment("KC_DB_URL", GetJdbcUrl(sql, dbKeycloak))
    .WithEnvironment("KC_DB_USERNAME", sqlUsername)
    .WithEnvironment("KC_DB_PASSWORD", sqlPassword)
    .WithReference(dbKeycloak).WaitFor(dbKeycloak);

// Expense Tracker API
builder.AddProject<Projects.CabaVS_ExpenseTracker_API>("aca-expensetrackerapi")
    .WithReference(dbExpenseTracker, "SqlDatabase").WaitFor(dbExpenseTracker)
    .WithReference(keycloak).WaitFor(keycloak);

await builder.Build().RunAsync();

static string GetJdbcUrl(IResourceBuilder<SqlServerServerResource> sql, IResourceBuilder<SqlServerDatabaseResource> db)
    => $"jdbc:sqlserver://{sql.Resource.Name}:{sql.Resource.PrimaryEndpoint.TargetPort};databaseName={db.Resource.DatabaseName};encrypt=false";
