using Dapper;
using Top2000.Api.Database;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.AddSqlServerClient("sql");

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new SqlDbConnectionFactory(builder.Configuration.GetConnectionString("Top2000")!));
builder.Services.AddSingleton(_ =>
    new DbInitializer(builder.Configuration.GetConnectionString("Top2000")!));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddScoped<DbInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.MapGet("/editions", async (IDbConnectionFactory dbConnectionFactory) =>
{
    var connection = await dbConnectionFactory.CreateConnectionAsync(default);
    var editions = await connection.QueryAsync<Edition>("select * from edition");

    return Results.Ok(editions);
})
.WithName("GetEditions")
.WithOpenApi();

var db = app.Services.GetRequiredService<DbInitializer>();
db.Initialize();

app.Run();

public sealed class Edition
{
    public required int Year { get; init; }
    public required DateTime StartUtcDateAndTime { get; init; }
    public required DateTime EndUtcDateAndTime { get; init; }
    public required bool HasPlayDateAndTime { get; init; }
}
