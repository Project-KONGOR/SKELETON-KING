namespace SKELETON_KING;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureRemotePassword;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ZORGATH;

class ServiceScopeFactory : IServiceScopeFactory
{
    private WebApplication? _app;
    public void SetApp(WebApplication app)
    {
        _app = app;
    }

    public IServiceScope CreateScope()
    {
        return _app!.Services.CreateScope();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        string connectionString = builder.Configuration.GetConnectionString("BOUNTY")!;
        builder.Services.AddDbContext<BountyContext>(options =>
        {
            options.UseSqlServer(connectionString, connection => connection.MigrationsAssembly("ZORGATH"));
        });

        ServiceScopeFactory serviceScopeFactory = new();
        ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions = new();

        // Used by pre_auth request and is replaced in unittests.
        builder.Services.AddSingleton((SrpServer srpServer, string verifier) => srpServer.GenerateEphemeral(verifier));

        builder.Services.AddSingleton(serviceScopeFactory);
        builder.Services.AddSingleton(
            new Dictionary<string, IClientRequesterHandler>()
            {
                {"pre_auth", new PreAuthHandler(serviceScopeFactory, srpAuthSessions) },
            }
        );

        var app = builder.Build();
        serviceScopeFactory.SetApp(app);

        app.MapControllers();
        app.Run();
    }
}