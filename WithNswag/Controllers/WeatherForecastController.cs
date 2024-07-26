using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NJsonSchema.NewtonsoftJson.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PolymorphController.Controllers;

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

[Newtonsoft.Json.JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
[KnownType(typeof(Dog))]
[KnownType(typeof(Cat))]
public abstract class AnimalBase
{
    public string Name { get; set; } = string.Empty;
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

