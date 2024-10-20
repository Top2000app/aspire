var builder = DistributedApplication.CreateBuilder(args);


var sqlPassword = builder.AddParameter("sql-password", secret: true);
var sqlServer = builder
    .AddSqlServer("sql", sqlPassword, port: 1234)
    //.WithDataVolume("DefaultVolume")
    .WithHealthCheck();

var sqlDatabase = sqlServer.AddDatabase("Top2000");

builder.AddProject<Projects.Top2000_Api>("Top2000-API")
     .WithReference(sqlDatabase)
     .WaitFor(sqlServer)
     ;

builder.Build().Run();
