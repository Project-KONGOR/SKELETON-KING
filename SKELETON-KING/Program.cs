namespace SKELETON_KING;

using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using ZORGATH;

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

        ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions = new();

        builder.Services.AddSingleton<ICookieValidator, CookieValidator>();
        builder.Services.AddSingleton<IReadOnlyDictionary<string, IClientRequesterHandler>>(
            new Dictionary<string, IClientRequesterHandler>()
            {
                {"pre_auth", new PreAuthHandler(srpAuthSessions) },
            }
        );

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}
