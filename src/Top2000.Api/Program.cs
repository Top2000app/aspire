using Top2000.Api.Database;
using Top2000.Api.GraphQL;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services
    .AddTransient<EditionRepository>()
    .AddTransient<TrackRepository>()
    .AddTransient<TrackListingRepository>();

builder.Services.AddGraphQLServer()
    .ModifyRequestOptions(options =>
    {
        options.IncludeExceptionDetails = builder.Environment.IsDevelopment();
    })
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
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

app.Run();

