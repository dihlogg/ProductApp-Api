namespace WavesOfFoodDemo.Server.Dtos
{
    public class CartInfoCreateDto
    {
        public string? Status {  get; set; }
        public DateTime? DateOrder { get; set; }
        public Guid UserId { get; set; }
    }
}
