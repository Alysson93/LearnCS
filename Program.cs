using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

app.MapGet("/", () => "Minha primeira página dotnet!");

app.MapGet("/addheader", (HttpResponse response) => {
	response.Headers.Add("Teste", "Ok");
	return new { Name = "Violão", Price = 569 };
});

app.MapGet("/testquery", ([FromQuery]string dateStart, [FromQuery]string dateEnd) => {
	return dateStart + " - " + dateEnd;
});

app.MapGet("/productheader", (HttpRequest request) => {
	return request.Headers["product-code"].ToString();
});

app.MapGet("/products/{code}", ([FromRoute] string code) => {
	var product = ProductRepository.GetBy(code);
	if (product != null) return Results.Ok(product);
	return Results.NotFound();
});

app.MapPost("/products", (Product product) => {
	ProductRepository.Add(product);
	return Results.Created($"/products/{product.Code}", product.Code);
});

app.MapPut("/products", (Product product) => {
	var productSaved = ProductRepository.GetBy(product.Code);
	productSaved.Name = product.Name;
	productSaved.Price = product.Price;
	return Results.Ok();
});

app.MapDelete("products/{code}", ([FromRoute]string code) => {
	var product = ProductRepository.GetBy(code);
	ProductRepository.Remove(product);
	return Results.Ok();
});

if (app.Environment.IsStaging())
{
	app.MapGet("/config/database", (IConfiguration configuration) => {
		return Results.Ok($"{configuration["database:connection"]}/{configuration["database:port"]}");
	});
}

app.Run();

/*****************************************/

public class Product
{
	public string Code { get; set; }
	public string Name { get; set; }
	public float Price { get; set; }
}

/*****************************************/

public static class ProductRepository
{
	public static List<Product> Products { get; set; } = new List<Product>();

	public static void Init(IConfiguration configuration)
	{
		var products = configuration.GetSection("Products").Get<List<Product>>();
		Products = products;
	}

	public static void Add(Product product)
	{
		//if (Products == null) Products = new List<Product>();
		Products.Add(product);
	}

	public static Product GetBy(string code)
	{
		return Products.FirstOrDefault(p => p.Code == code);
	}

	public static void Remove(Product product)
	{
		Products.Remove(product);
	}
}
