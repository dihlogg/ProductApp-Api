namespace WavesOfFoodDemo.Server.Entities
{
    public class CartDetails : BaseEntities
    {
        public Guid ProductId { get; set; }

        public ProductInfo ProductInfo { get; set; }

        public Guid CartId { get; set; }

        public CartInfo CartInfo { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
