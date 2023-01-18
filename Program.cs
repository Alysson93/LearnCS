var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Minha primeira página dotnet!");

app.MapPost("/", () => new {Name = "Alysson Pereira", Age = 29});

app.MapGet("/AddHeader", (HttpResponse response) => {
	response.Headers.Add("teste", "Alysson");
	return new {Name = "Alysson Pereira", Age = 29};
});

app.MapPost("/saveProduct", (Product product) => {
	return product.Code + " - " + product.Name;
});

app.Run();

public class Product
{

	public string Code { get; set; }
	public string Name { get; set; }

}
