using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Permet de sérialiser enums et objets JSON propres
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

const string JsonFilePath = "products.json";
static List<Product> LoadProducts()
{
    if (!File.Exists(JsonFilePath))
        return new List<Product>();

    string json = File.ReadAllText(JsonFilePath);
    List<Product>? list = JsonSerializer.Deserialize<List<Product>>(json);
    return list ?? new List<Product>();
}

static void SaveProducts(List<Product> products)
{
    string json = JsonSerializer.Serialize(products, new JsonSerializerOptions
    {
        WriteIndented = true,          // Format lisible
        PropertyNamingPolicy = null    // Respecte la casse des propriétés
    });
    File.WriteAllText(JsonFilePath, json);
}

List<Product> products = LoadProducts();
int nextId = products.Any() ? products.Max(p => p.Id) + 1 : 1;

// -------------------------
// Endpoints REST
// -------------------------

app.MapGet("/products", () => Results.Ok(products));

app.MapGet("/products/{id:int}", (int id) =>
{
    Product? product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/products", (Product newProduct) =>
{
    newProduct.Id = nextId++;
    products.Add(newProduct);
    SaveProducts(products);
    return Results.Created($"/products/{newProduct.Id}", newProduct);
});

app.MapPut("/products/{id:int}", (int id, Product updated) =>
{
    Product? existing = products.FirstOrDefault(p => p.Id == id);
    if (existing is null)
        return Results.NotFound();

    existing = updated;
    SaveProducts(products);
    return Results.NoContent();
});

app.MapDelete("/products/{id:int}", (int id) => {
    Product? existing = products.FirstOrDefault(p => p.Id == id);
    if (existing is null)
        return Results.NotFound();

    products.Remove(existing);
    SaveProducts(products);
    return Results.NoContent(); 
});

app.Run();

public record Product {
    public required int Id { get; set; }
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }
}