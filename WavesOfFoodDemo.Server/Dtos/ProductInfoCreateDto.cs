namespace WavesOfFoodDemo.Server.Dtos;

public class ProductInfoCreateDto
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public Guid? CategoryId { get; set; }
    public List<ProductImageCreateDto> ProductImages { get; set; } = new List<ProductImageCreateDto>();
}