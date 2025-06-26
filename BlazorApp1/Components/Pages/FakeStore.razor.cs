using System.Net;
using System.Text.Json;
namespace BlazorApp1.Components.Pages;

public class Product {
    public required int Id { get; set; }
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }
}

public partial class FakeStore {
    private readonly HttpClient _httpClient = new();
    private bool _isLoading;
    private List<Product> _products = new();
    private int _selectedId;
    private string _userResponse = string.Empty;

    protected override Task OnInitializedAsync() {
        LoadProducts().Wait();
        return base.OnInitializedAsync();
    }

    private async Task LoadProducts() {
        _isLoading = true;
        try {
            _products.Clear();
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
        await AddNewProduct(); 
        _products=await GetAllProducts();
    }

    private Task AddNewProduct() {
        var random = new Random();
        Product newProduct = new Product {
            Id = 1,
            Title = "Test",
            Price = random.Next(10)+(decimal)random.NextSingle(),
            Description = "A test product",
            Category =  "category",
            Image = ""
        };
        Console.WriteLine($"Added new product : {newProduct.Title} - {newProduct.Description}");
        HttpContent content = new StringContent(JsonSerializer.Serialize(newProduct), System.Text.Encoding.UTF8, "application/json");
        var response=_httpClient.PostAsync("http://localhost:5136/products", content);
        response.Result.EnsureSuccessStatusCode();
        return Task.CompletedTask;
    }

    private async void OnClickDelete() {
        await Delete();
        _products=await GetAllProducts();
    }

    private Task Delete() {
        try {
            var response = _httpClient.DeleteAsync($"http://localhost:5136/products/{_selectedId}");
            if (response.Result.StatusCode == HttpStatusCode.NotFound) {
                _userResponse="Please enter a valid Id.";
                return Task.CompletedTask;
            }
            response.Result.EnsureSuccessStatusCode();
            _userResponse = $"The product n°{_selectedId} has been deleted.";
            return Task.CompletedTask;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}