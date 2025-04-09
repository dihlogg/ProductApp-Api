namespace WavesOfFoodDemo.Server.Dtos.ChatBot
{
    public class ChatRequestDto
    {
        public string Message { get; set; }
        public string ConversationId { get; set; }
        public string ContactId { get; set; }
    }
}
