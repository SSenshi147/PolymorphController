using System.Text.Json.Serialization;

namespace PolymorphController;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All);
        //builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddControllers();
        //builder.Services.AddOpenApiDocument(opt =>
        //{
        //});

        builder.Services.AddSwaggerGen(opt =>
        {
            opt.UseOneOfForPolymorphism();
            opt.UseAllOfForInheritance();
            opt.SelectDiscriminatorNameUsing(_ => "$type");
            opt.SelectDiscriminatorValueUsing(_ =>
            {
                var baseType = _;

                string? output = null;

                while (baseType != null && output == null)
                {
                    output =
                        Attribute
                            .GetCustomAttributes(baseType, typeof(JsonDerivedTypeAttribute), inherit: true)
                            .Cast<JsonDerivedTypeAttribute>()
                            .Where(a => a.DerivedType == _.UnderlyingSystemType)
                            .Select(a => a.TypeDiscriminator?.ToString())
                            .Where(d => d != null)
                            .Distinct()
                            .SingleOrDefault();

                    baseType = baseType.BaseType;
                }

                return output;
            });
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseOpenApi();
            //app.UseSwaggerUi();

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
