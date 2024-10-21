using Top2000.Api.Database;
using Top2000.Api.GraphQL;
using Top2000.Api.GraphQL.GraphDb;
using Top2000.Api.GraphQL.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services
    .AddTransient<EditionRepository>()
    .AddTransient<TrackRepository>()
    .AddTransient<ListingRepository>()
    .AddSingleton<InMemoryDatabaseGraphDatabase>()
    ;

builder.Services.AddGraphQLServer()
    .ModifyRequestOptions(options =>
    {
        options.IncludeExceptionDetails = builder.Environment.IsDevelopment();
    })
    .AddQueryType<Query>()
    // .AddMutationType<Mutation>()
    ;

builder.AddSqlServerClient("sql");

builder.Services.AddTransient<IDbConnectionFactory>(_ =>
    new SqlDbConnectionFactory(builder.Configuration.GetConnectionString("Top2000")!));
builder.Services.AddSingleton(_ =>
    new DbInitializer(builder.Configuration.GetConnectionString("Top2000")!));


var app = builder.Build();

app.MapGraphQL();
app.UseHttpsRedirection();

//app.MapDefaultEndpoints();

//app.MapGet("/editions", async (IDbConnectionFactory dbConnectionFactory) =>
//{
//    var connection = await dbConnectionFactory.CreateConnectionAsync(default);
//    var editions = await connection.QueryAsync<Edition>("select * from edition");

//    return Results.Ok(editions);
//})
//.WithName("GetEditions")
//.WithOpenApi();

var db = app.Services.GetRequiredService<DbInitializer>();
db.Initialize();

var sqlConnectionFactory = app.Services.GetRequiredService<IDbConnectionFactory>();

var graphDb = app.Services.GetRequiredService<InMemoryDatabaseGraphDatabase>();
await graphDb.InitialiseAsync(sqlConnectionFactory);

app.Run();

