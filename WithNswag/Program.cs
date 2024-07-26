
using Namotion.Reflection;

namespace WithNswag;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().AddNewtonsoftJson();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiDocument(x =>
        {
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi(x =>
            {
            });
            app.UseSwaggerUi(x =>
            {
            });
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
