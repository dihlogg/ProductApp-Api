namespace WavesOfFoodDemo.Server.Dtos
{
    public class CartDetailsCreateDto
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
    }
}
