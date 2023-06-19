namespace CDRAPI.DTOs
{
    public class SingleResponse
    {
        public SingleResponse(string data)
        {
            Data = data;
        }

        public string Data { get; set; }
    }
}
