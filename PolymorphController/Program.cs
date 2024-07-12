namespace PolymorphController;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All);
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddOpenApiDocument();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
