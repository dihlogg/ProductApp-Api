namespace WavesOfFoodDemo.Server.Dtos
{
    public class CartDetailsDto : CartDetailsCreateDto
    {
        public Guid Id { get; set; }
    }
    public class CartDetailsRequestDto
    {
        public Guid? UserId { get; set; }
        public List<CartDetailsRequestCreateDto>? Products { get; set; }
    }
    public class CartDetailsRequestCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
