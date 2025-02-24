namespace WavesOfFoodDemo.Server.Dtos.CartDetails;
public class CartDetailInfoDto
{
    public Guid? CartInfoId { get; set; }
    public string Status { get; set; }
    public DateTime DateOrder { get; set; }
    public Guid UserId { get; set; }
    public  List<CartDetailDto> CartDetailDtos { get; set; }
}

public class CartDetailDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
