using WavesOfFoodDemo.Server.AppSettings;

namespace WavesOfFoodDemo.Server.Dtos.CartDetails;
public class CartDetailInfoDto
{
    public Guid? CartInfoId { get; set; }
    public string? Status { get; set; } = StatusOrderConst.NewConst;
    public DateTime DateOrder { get; set; }
    public Guid UserId { get; set; }
    public List<CartDetailDto> CartDetailDtos { get; set; }
}

public class CartDetailDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public Guid UserId { get; set; }
}

public class UpdateStatusCartDetailDto
{
    public Guid CartInfoId { get; set; }
    public string? Status { get; set; }
}
public class UpdateCartQuantityDto
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
