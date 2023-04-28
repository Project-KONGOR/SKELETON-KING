using ZORGATH;

namespace SKELETON_KING;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        builder.Services.AddSingleton<IReadOnlyDictionary<string, IClientRequesterHandler>>(new Dictionary<string, IClientRequesterHandler>());

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}
