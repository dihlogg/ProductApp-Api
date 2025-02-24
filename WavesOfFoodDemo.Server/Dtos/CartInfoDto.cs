namespace WavesOfFoodDemo.Server.Dtos
{
    public class CartInfoDto : CartInfoCreateDto
    {
        public Guid Id { get; set; }
        public CartDetailsDto CartDetailsDto { get; set; } = new CartDetailsDto()
        {
        };
    }
}
