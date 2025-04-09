using Microsoft.ML.Data;

namespace WavesOfFoodDemo.Server.Dtos.Clustering
{
    public class ProductFeatureDto : ProductFeatureCreateDto
    {
        public Guid Id { get; set; }
    }
    public class ProductFeatureCreateDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public Guid? CategoryId { get; set; }
        public List<ProductImageCreateDto> ProductImages { get; set; } = new List<ProductImageCreateDto>();

        // recommend criteria
        public string? CpuType { get; set; }
        public string? RamType { get; set; }
        public string? RomType { get; set; }
        public string? ScreenSize { get; set; }
        public string? BateryCapacity { get; set; }
        public string? DetailsType { get; set; }
        public string? ConnectType { get; set; }
    }
    public class ProductFeatureMLDto
    {
        public float Price { get; set; }
        // recommend criteria
        public string? CpuType { get; set; }
        public string? RamType { get; set; }
        public string? RomType { get; set; }
        public string? ScreenSize { get; set; }
        public string? BateryCapacity { get; set; }
        public string? DetailsType { get; set; }
        public string? ConnectType { get; set; }
        public int PriceLevel { get; set; }
    }
    public class ProductPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint ClusterId { get; set; }
    }
}
