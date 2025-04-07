namespace WavesOfFoodDemo.Server.Entities
{
    public class ProductImage : BaseEntities
    {
        public Guid? ProductInfoId { get; set; }
        public ProductInfo? ProductInfos { get; set; }
        public string? ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? ProductInfoHistoryId { get; set; }
        public ProductInfoHistory? ProductInfoHistories { get; set; }
    }
}
