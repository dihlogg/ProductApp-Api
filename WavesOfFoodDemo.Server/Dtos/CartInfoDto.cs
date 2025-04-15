namespace WavesOfFoodDemo.Server.Dtos
{
    public class CartInfoDto : CartInfoCreateDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
