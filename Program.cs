using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Minha primeira página dotnet!");

app.MapGet("/addheader", (HttpResponse response) => {
	response.Headers.Add("Teste", "Ok");
	return new { Name = "Violão", Price = 569 };
});

app.MapGet("/product", ([FromQuery]string dateStart, [FromQuery]string dateEnd) => {
	return dateStart + " - " + dateEnd;
});

app.MapGet("/product/{code}", ([FromRoute] string code) => {
	var product = ProductRepository.GetBy(code);
	return product;
});

app.MapGet("/productheader", (HttpRequest request) => {
	return request.Headers["product-code"].ToString();
});

app.MapPost("/product", (Product product) => {
	ProductRepository.Add(product);
});

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
	public static List<Product> Products { get; set; }

	public static void Add(Product product)
	{
		if (Products == null) Products = new List<Product>();
		Products.Add(product);
	}

	public static Product GetBy(string code)
	{
		return Products.FirstOrDefault(p => p.Code == code);
	}
}
