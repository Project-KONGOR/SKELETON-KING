namespace SKELETON_KING;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UNDEAD_WARRIOR;

public class HelloHandler : IClientRequesterHandler
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IActionResult> HandleRequest(Dictionary<string, string> formData)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new OkObjectResult("hello!");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        builder.Services.AddSingleton(
            new Dictionary<string, IClientRequesterHandler>()
            {
                {"hello", new HelloHandler() }
            }
        );

        var app = builder.Build();
        app.MapControllers();

        app.Run();
    }
}
