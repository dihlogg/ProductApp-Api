namespace WavesOfFoodDemo.Server.Dtos.ChatBot
{
    public class ChatRequestDto
    {
        public string message { get; set; }
        public Guid userId { get; set; }
    }
}
