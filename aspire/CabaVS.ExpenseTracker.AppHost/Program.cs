IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<SqlServerServerResource> sql = builder.AddSqlServer("sql-cabavsprojectshub", port: 1433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<SqlServerDatabaseResource> db = sql.AddDatabase("sqldb-expensetracker");

builder.AddProject<Projects.CabaVS_ExpenseTracker_API>("aca-expensetrackerapi")
    .WithReference(db, "SqlDatabase")
    .WaitFor(db);

await builder.Build().RunAsync();
