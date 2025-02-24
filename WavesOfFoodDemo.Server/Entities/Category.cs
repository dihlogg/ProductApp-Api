namespace WavesOfFoodDemo.Server.Entities
{
    public class Category : BaseEntities
    {
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public ICollection<ProductInfo> ProductInfos { get; set; }
    }
}
