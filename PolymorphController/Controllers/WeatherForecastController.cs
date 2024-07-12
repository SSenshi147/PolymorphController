using Microsoft.AspNetCore.Mvc;
using NJsonSchema.NewtonsoftJson.Converters;
using System.Runtime.Serialization;

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
        if (animalBase is Mouse mouse)
            return Task.FromResult(new MouseProcessor().Process(mouse));
        throw new NotImplementedException();
    }

    [HttpPost("Cat")]
    public Task<string> ProcessCat([FromBody] Cat cat)
        => Task.FromResult(new CatProcessor().Process(cat));

    [HttpPost("Dog")]
    public Task<string> ProcessDog([FromBody] Dog dog)
        => Task.FromResult(new DogProcessor().Process(dog));
}

[Newtonsoft.Json.JsonConverter(typeof(JsonInheritanceConverter), "$type")]
[KnownType(typeof(Dog))]
[KnownType(typeof(Cat))]
[KnownType(typeof(Mouse))]
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

public class Mouse : AnimalBase
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

public class MouseProcessor : Processor<Mouse>
{
    public override string Process(Mouse animal)
    {
        return $"this is mouse, name {animal.Name}, !color: {animal.Color}";
    }
}