using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;


//using NJsonSchema.NewtonsoftJson.Converters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PolymorphController.Controllers;

//public class MyRequestBodyFilter : IRequestBodyFilter
//{
//    public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context)
//    {
//        ;
//    }
//}

//public class MySchemaFilter : ISchemaFilter
//{
//    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
//    {
//        var dc = schema.Discriminator;
//        if (context.MemberInfo?.ReflectedType?.IsAbstract ?? false)
//        {
//            ;
//        }
//    }
//}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpPost]
    public Task<string> Process([FromBody] AnimalBase animalBase)
    {
        if (animalBase is Cat cat)
            return Task.FromResult(new CatProcessor().Process(cat));
        if (animalBase is Dog dog)
            return Task.FromResult(new DogProcessor().Process(dog));
        throw new NotImplementedException();
    }
}

//[Newtonsoft.Json.JsonConverter(typeof(JsonInheritanceConverter), "$type")]
//[KnownType(typeof(Dog))]
//[KnownType(typeof(Cat))]
[JsonDerivedType(typeof(Dog), "dog")]
[JsonDerivedType(typeof(Cat), "cat")]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "animalType")]
[SwaggerSubType(typeof(Cat), DiscriminatorValue = "cat")]
[SwaggerSubType(typeof(Dog), DiscriminatorValue = "dog")]
[SwaggerDiscriminator("animalType")]
public abstract class AnimalBase
{
    public AnimalType AnimalType { get; set; }
    public string Name { get; set; } = string.Empty;
}

public enum AnimalType
{
    dog, cat
}

public class Dog : AnimalBase
{
    public bool GoodBoi { get; set; }
}

public class Cat : AnimalBase
{
    public string Color { get; set; } = string.Empty;
}

public abstract class Processor<T> where T : AnimalBase
{
    public abstract string Process(T animal);
}

public class DogProcessor : Processor<Dog>
{
    public override string Process(Dog animal)
    {
        return $"this is a dog, name {animal.Name}, gudboi {animal.GoodBoi}";
    }
}

public class CatProcessor : Processor<Cat>
{
    public override string Process(Cat animal)
    {
        return $"this is cat, name {animal.Name}, color: {animal.Color}";
    }
}

public class Asd : IMultipleExamplesProvider<AnimalBase>
{
    public IEnumerable<SwaggerExample<AnimalBase>> GetExamples()
    {
        yield return new SwaggerExample<AnimalBase>()
        {
            Name = "Cat",
            Description = "Cat",
            Summary = "Cat",
            Value = new Cat
            {
                AnimalType = AnimalType.cat,
                Color = "orange",
                Name = "sanyi",
            }
        };

        yield return new SwaggerExample<AnimalBase>()
        {
            Name = "Dog",
            Description = "Dog",
            Summary = "Dog",
            Value = new Dog
            {
                AnimalType = AnimalType.dog,
                GoodBoi = true,
                Name = "kutyi",
            }
        };
    }
}