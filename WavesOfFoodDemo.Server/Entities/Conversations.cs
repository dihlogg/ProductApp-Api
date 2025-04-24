namespace WavesOfFoodDemo.Server.Entities
{
    public class Conversations : BaseEntities
    {
        public string Message { get; set; }
        public string Sender { get; set; }
        public DateTime Timestamp { get; set; }
        public string? ProductsJson { get; set; }
        public Guid UserId { get; set; }
        public UserInfo UserInfos { get; set; }
    }
}
