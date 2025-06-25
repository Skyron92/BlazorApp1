using System.Text.Json;
namespace BlazorApp1.Components.Pages;

public class Product {
    public Product(int id, string title, decimal price, string description, string category, string image) {
        Id = id;
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
    }
    
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } 
    public string Category { get; set; }
    public string Image { get; set; }
}

public partial class FakeStore {
    private readonly HttpClient _httpClient = new();
    private bool _isLoading;
    private List<Product> _products = new();

    private int counter = 0;

    protected override Task OnInitializedAsync() {
        LoadProducts().Wait();
        return base.OnInitializedAsync();
    }

    private async Task LoadProducts() {
        _isLoading = true;
        try {
            _products = await GetAllProducts();
            _isLoading = false;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task<List<Product>> GetAllProducts() {
        var response=_httpClient.GetAsync("http://localhost:5136/products");
        response.Result.EnsureSuccessStatusCode();
        Console.WriteLine("Get response from fakestoreapi.com");
        
        string content = await response.Result.Content.ReadAsStringAsync();
        List<Product>? products = JsonSerializer.Deserialize<List<Product>>(content);
        foreach (var product in products?? new List<Product>()) {
            Console.WriteLine($"Product found : {product.Title} - {product.Description} - {product.Price} - {product.Id}");
        }
        return products?? new List<Product>();
    }

    private async Task OnClick() {
        counter++;
        await AddNewProduct(); 
        await GetAllProducts();
    }

    private Task AddNewProduct() {
        var random = new Random();
        Product newProduct = new (1, "Test",random.Next(10)+(decimal)random.NextSingle(),"A test product", "category", "");
        Console.WriteLine($"Added new product : {newProduct.Title} - {newProduct.Description}");
        HttpContent content = new StringContent(JsonSerializer.Serialize(newProduct), System.Text.Encoding.UTF8, "application/json");
        var response=_httpClient.PostAsync("http://localhost:5136/products", content);
        response.Result.EnsureSuccessStatusCode();
        return Task.CompletedTask;
    }
}