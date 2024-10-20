using DbUp;

namespace Top2000.Api.Database;

public class DbInitializer
{
    private readonly string connectionString;

    public DbInitializer(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void Initialize()
    {
        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptEmbeddedInDataLibrary()
            .WithTransactionPerScript()
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw new InvalidOperationException("DB not working", result.Error);
        }
    }
}
