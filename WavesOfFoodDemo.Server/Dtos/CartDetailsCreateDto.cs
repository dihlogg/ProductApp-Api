namespace WavesOfFoodDemo.Server.Dtos
{
    public class CartDetailsCreateDto
    {
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
