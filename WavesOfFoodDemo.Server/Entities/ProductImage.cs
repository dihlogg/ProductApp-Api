namespace WavesOfFoodDemo.Server.Entities
{
    public class ProductImage : BaseEntities
    {
        public Guid? ProductInfoId { get; set; }
        public ProductInfo? ProductInfos { get; set; }
        public string? ImageUrl { get; set; }
    }
}
