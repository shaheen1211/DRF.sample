using DRF.sample;
using System.Text.Json;

Console.WriteLine("UserName :");
var userName = Console.ReadLine();
Console.WriteLine("Password");
var password = Console.ReadLine();

var user = new { username = userName, password };

var apiClient = new ApiClient("https://dummyjson.com");

var responseBody = await apiClient.PostAsync("/auth/login", JsonSerializer.Serialize(user));
var responseObject = JsonSerializer.Deserialize<JsonElement>(responseBody);
string token = responseObject.GetProperty("token").GetString();

Console.WriteLine(token);
var searchResult = await apiClient.GetAsync<ProductSearchResult>("products/category/smartphones");

List<Product> expensivePhones = new List<Product>();
if (searchResult != null && searchResult.Products != null)
{
    expensivePhones = searchResult.Products.OrderByDescending(r => r.Price)
        .Take(3).ToList();
}

DisplayPhones(expensivePhones);

bool validPercentageEntered = false;
decimal inputPercentage = 0;

Console.WriteLine("Enter Percentage : ");
while (!validPercentageEntered)
{
    var input = Console.ReadLine();
    validPercentageEntered = decimal.TryParse(input, out inputPercentage);
}

var updatedPhoneList = new List<Product>();
foreach (var product in expensivePhones)
{
    var updatedPrice = new { price = product.Price * (1 + inputPercentage / 100) };
    var updatedProduct = await apiClient.PutAsync<Product>($"/products/{product.Id}", JsonSerializer.Serialize(updatedPrice));

    updatedPhoneList.Add(updatedProduct);
}

DisplayPhones(updatedPhoneList);

Console.WriteLine("Product fetch and update complete");
Console.ReadLine();

static void DisplayPhones(List<Product> smartPhones)
{
    Console.WriteLine($"Brand       Price   Title");
    foreach (var product in smartPhones)
    {
        Console.WriteLine($"{product.Brand}     {product.Price}     {product.Title}");
    }
}