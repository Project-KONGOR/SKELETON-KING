namespace TRANSMUTANSTEIN;

// Custom IServiceScope that allows us to e.g inject custom BountyContext.
class ServiceScopeForTesting : IServiceScope
{
    private readonly string _identifier;
    private readonly InMemoryDatabaseRoot _inMemoryDatabaseRoot;
    public IServiceProvider ServiceProvider { get; private set; }

    public ServiceScopeForTesting(string identifier, InMemoryDatabaseRoot inMemoryDatabaseRoot, Dictionary<Type, object> singletons)
    {
        _identifier = identifier;
        _inMemoryDatabaseRoot = inMemoryDatabaseRoot;

        ServiceCollection serviceCollection = new();
        foreach (var kvp in singletons)
        {
            serviceCollection.AddSingleton(kvp.Key, kvp.Value);
        }

        serviceCollection.AddEntityFrameworkInMemoryDatabase();
        serviceCollection.AddDbContext<BountyContext>((sp, options) =>
        {
            options.UseInMemoryDatabase(_identifier, _inMemoryDatabaseRoot, b => b.EnableNullChecks(false));
            options.UseInternalServiceProvider(sp);
        });
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

// Custom IServiceScopeFactory that allows us to e.g inject custom Singletons.
class ServiceScopeFactoryForTesting : IServiceScopeFactory
{
    private readonly string _identifier = Guid.NewGuid().ToString();
    private readonly InMemoryDatabaseRoot _inMemoryDatabaseRoot = new();
    private readonly Dictionary<Type, object> _singletons = new();

    public void AddSingleton<TService>(TService impl)
    {
        _singletons[typeof(TService)] = impl!;
    }

    public IServiceScope CreateScope()
    {
        return new ServiceScopeForTesting(_identifier, _inMemoryDatabaseRoot, _singletons);
    }
}
