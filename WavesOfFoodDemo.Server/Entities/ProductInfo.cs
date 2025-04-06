namespace WavesOfFoodDemo.Server.Entities;

public class ProductInfo : BaseEntities
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Categories { get; set; }
    public IList<CartDetails> CartDetails { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }

    // recommend criteria
    public string? CpuType { get; set; }
    public string? RamType { get; set; }
    public string? RomType { get; set; }
    public string? ScreenSize { get; set; }
    public string? BateryCapacity { get; set; }
    public string? DetailsType { get; set; }
    public string? ConnectType { get; set; }
}
