namespace TRANSMUTANSTEIN;

public class ControllerContextForTesting : ControllerContext
{
    public ControllerContextForTesting()
    {
        SqliteConnection connection = new("Data Source=:memory:");
        ServiceCollection serviceCollection = new();
        serviceCollection.AddDbContextFactory<BountyContext>((sp, options) =>
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

        // Make sure RemoteIpAddress is not null.
        string arbitraryIpAdress = "192.168.13.12";
        HttpContext.Connection.RemoteIpAddress = IPAddress.Parse(arbitraryIpAdress);
    }
}
