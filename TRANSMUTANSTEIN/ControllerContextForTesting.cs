namespace TRANSMUTANSTEIN;

public class ControllerContextForTesting : ControllerContext
{
    public ControllerContextForTesting()
    {
        SqliteConnection connection = new("Data Source=:memory:");
        ServiceCollection serviceCollection = new();
        serviceCollection.AddDbContext<BountyContext>((sp, options) =>
        {
            options.UseSqlite(connection);
        });

        ServiceProvidersFeature serviceProvidersFeature = new()
        {
            RequestServices = serviceCollection.BuildServiceProvider()
        };

        // Apparently required to create schema.
        BountyContext bountyContext = serviceProvidersFeature.RequestServices.GetRequiredService<BountyContext>();
        bountyContext.Database.OpenConnection();
        bountyContext.Database.EnsureCreated();

        IFeatureCollection features = new FeatureCollection();
        features.Set<IServiceProvidersFeature>(serviceProvidersFeature);

        HttpContext = new DefaultHttpContext(features);
        HttpContext.Connection.RemoteIpAddress = IPAddress.Parse("192.168.13.12");
    }
}
