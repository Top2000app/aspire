using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Top2000.Api.GraphQL;
using Snapshooter.MSTest;
using HotChocolate.Execution;

namespace Top2000.Api.Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public async Task TestMethod1()
    {
        var schema = await new ServiceCollection()
            .AddGraphQL()
            .AddQueryType<Query>()
            .BuildSchemaAsync();

        Verify(schema.ToString());
    }

    [TestMethod]
    public async Task LatestEdition()
    {
        var schema = new ServiceCollection()
            .AddGraphQL()
            .AddQueryType<Query>();
         
        var result = await schema.ExecuteRequestAsync("query{latest{year}}");
        
        
    }
}