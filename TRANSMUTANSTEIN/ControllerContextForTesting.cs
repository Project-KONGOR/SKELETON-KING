namespace TRANSMUTANSTEIN;

public class ControllerContextForTesting : ControllerContext
{
    public ControllerContextForTesting()
    {
        string identifier = Guid.NewGuid().ToString();
        InMemoryDatabaseRoot inMemoryDatabaseRoot = new();

        ServiceCollection serviceCollection = new();
        serviceCollection.AddEntityFrameworkInMemoryDatabase();
        serviceCollection.AddDbContext<BountyContext>((sp, options) =>
        {
            options.UseInMemoryDatabase(identifier, inMemoryDatabaseRoot, b => b.EnableNullChecks(false));
            options.UseInternalServiceProvider(sp);
        });

        ServiceProvidersFeature serviceProvidersFeature = new()
        {
            RequestServices = serviceCollection.BuildServiceProvider()
        };

        IFeatureCollection features = new FeatureCollection();
        features.Set<IServiceProvidersFeature>(serviceProvidersFeature);

        HttpContext = new DefaultHttpContext(features);
    }
}
