namespace SKELETON_KING;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        var app = builder.Build();
        app.Run();
    }
}
