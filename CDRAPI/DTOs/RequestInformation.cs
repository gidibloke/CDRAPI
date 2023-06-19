namespace CDRAPI.DTOs
{
    public class RequestInformation
    {
        public Guid Id { get; set; }
        public DateTime? RequestStarted { get; set; }
        public DateTime? RequestEnded { get; set; }
        public string Comments { get; set; }
    }
}
