namespace WavesOfFoodDemo.Server.Dtos;
public class CartHistoryDto
{
    public Guid Id { get; set; }
    public string DateOrder { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public IEnumerable<CartdetailHistoryDto> CartDetails { get; set; }
}

public class CartdetailHistoryDto
{
    public List<ProductImageCreateDto> ProductImages { get; set; } = new List<ProductImageCreateDto>();
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
