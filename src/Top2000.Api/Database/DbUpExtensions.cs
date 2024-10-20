using DbUp.Builder;
using Top2000.Data;

namespace Top2000.Api.Database;
public static class DbUpExtensions
{
    public static UpgradeEngineBuilder WithScriptEmbeddedInDataLibrary(this UpgradeEngineBuilder builder)
        => builder.WithScripts(new Top2000DataScriptProvider(new Top2000Data()));
}
