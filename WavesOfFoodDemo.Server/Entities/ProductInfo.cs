namespace WavesOfFoodDemo.Server.Entities;

public class ProductInfo : BaseEntities
{
    public string? Name { get; set; }

    public decimal Price { get; set; }

    public string? ImageMenu { get; set; }

    public string?  ImageDetail { get; set; }

    public string? Description { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Categories { get; set; }
    public IList<CartDetails> CartDetails { get; set; }
}
