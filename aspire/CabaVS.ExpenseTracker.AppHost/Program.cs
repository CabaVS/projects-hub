using Aspire.Hosting.Azure;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

// Parameters
IResourceBuilder<ParameterResource> sqlUsername = builder.AddParameter("sql-cabavsprojectshub-username", true);
IResourceBuilder<ParameterResource> sqlPassword = builder.AddParameter("sql-cabavsprojectshub-password", true);
IResourceBuilder<ParameterResource> keycloakUsername = builder.AddParameter("aca-keycloak-login", true);
IResourceBuilder<ParameterResource> keycloakPassword = builder.AddParameter("aca-keycloak-password", true);

// Storage Account (emulated with Azurite)
IResourceBuilder<AzureStorageResource> azurite = builder.AddAzureStorage("stcabavsprojectshub")
    .RunAsEmulator(config => config
        .WithBlobPort(27000)
        .WithQueuePort(27001)
        .WithTablePort(27002)
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent));
IResourceBuilder<AzureBlobStorageResource> blobsResource = azurite.AddBlobs("blobs");

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
    .WithEnvironment("KEYCLOAK_ADMIN", keycloakUsername)
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", keycloakPassword)
    .WithReference(dbKeycloak).WaitFor(dbKeycloak);

// Expense Tracker API
builder.AddProject<Projects.CabaVS_ExpenseTracker_API>("aca-expensetrackerapi")
    .WithEnvironment("CVS_CONFIGURATION_FROM_AZURE_URL", "http://127.0.0.1:27000/devstoreaccount1/app-configs/proj-expensetracker.local.json")
    .WithReference(blobsResource).WaitFor(blobsResource)
    .WithReference(dbExpenseTracker, "SqlDatabase").WaitFor(dbExpenseTracker)
    .WithReference(keycloak).WaitFor(keycloak);

await builder.Build().RunAsync();

static string GetJdbcUrl(IResourceBuilder<SqlServerServerResource> sql, IResourceBuilder<SqlServerDatabaseResource> db)
    => $"jdbc:sqlserver://{sql.Resource.Name}:{sql.Resource.PrimaryEndpoint.TargetPort};databaseName={db.Resource.DatabaseName};encrypt=false";
