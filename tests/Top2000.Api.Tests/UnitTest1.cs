using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Top2000.Api.GraphQL;
using NSubstitute;
using Top2000.Api.GraphQL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Top2000.Api.Tests;

[TestClass]
[UsesVerify]
public partial class UnitTest1
{
    [TestMethod]
    public void XX()
    {
        Assert.AreEqual(1,1);
    }
    
    [TestMethod]
    public async Task TestMethod1()
    {
        var schema = await new ServiceCollection()
            .AddGraphQL()
            .AddQueryType<Query>()
            .BuildSchemaAsync();

        await Verify(schema.ToString());
    }

    [TestMethod]
    public async Task LatestEdition()
    {
        var schema = new ServiceCollection()
            .AddTransient<EditionRepository>()
            .AddTransient<TrackRepository>()
            .AddTransient<ListingRepository>()
            .AddGraphQL()
            .AddQueryType<Query>();
         
        var result = await schema.ExecuteRequestAsync("{ query{latest{year}} }");
        
        var opResult = result as OperationResult;
        await Verify(result.ToJson(), "json");
        Assert.AreEqual(ExecutionResultKind.SingleResult, opResult.Kind);
    }
    
    [TestMethod]
    public async Task GetMovieByIdWithMockingTest()
    {
        var mockMovieRepository = Substitute.For<EditionRepository>();
        mockMovieRepository.Latest().Returns(new Edition
        {
            Year = 2086
        });

        var schema = new ServiceCollection()
            .AddTransient(_ => mockMovieRepository)
            .AddGraphQL()
            .AddQueryType<Query>();

        var result = await schema.ExecuteRequestAsync("{query{latest{year}}}");

        var opResult = result as OperationResult;
        await Verify(result.ToJson(), "json");
        Assert.AreEqual(ExecutionResultKind.SingleResult, opResult.Kind);
    }
}