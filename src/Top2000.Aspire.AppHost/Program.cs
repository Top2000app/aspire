using NorthernNerds.Aspire.Neo4j.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("sql-password", secret: true);
var sqlServer = builder
    .AddSqlServer("sql", sqlPassword, port: 1234)
    .WithHealthCheck();

var sqlDatabase = sqlServer.AddDatabase("Top2000");

var neo4jdbUsername = builder.AddParameter("neo4j-user");
var neo4jdbPassword = builder.AddParameter("neo4j-pass");

var neo4jDb = builder
    .AddNeo4j("graph-db", neo4jdbUsername, neo4jdbPassword);


builder.AddProject<Projects.Top2000_Api>("Top2000-API")
     .WithReference(sqlDatabase)
     .WaitFor(sqlServer)
     ;

builder.Build().Run();
