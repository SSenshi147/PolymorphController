using PolymorphController.Controllers;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;

namespace PolymorphController;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All);
        //builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        //builder.Services.AddOpenApiDocument(opt =>
        //{
        //});

        builder.Services.AddSwaggerExamplesFromAssemblyOf<Asd>();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.EnableAnnotations(true, true);
            opt.UseOneOfForPolymorphism();
            opt.UseAllOfForInheritance();
            opt.SelectDiscriminatorNameUsing(_ => "animalType");
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
            opt.ExampleFilters();
            //opt.AddRequestBodyFilterInstance<MyRequestBodyFilter>(new MyRequestBodyFilter());
            //opt.AddSchemaFilterInstance<MySchemaFilter>(new MySchemaFilter());
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
