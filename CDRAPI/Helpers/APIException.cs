using System.Net;

namespace CDRAPI.Helpers
{
    public class APIException : Exception
    {
        public APIException(HttpStatusCode code, object errors)
        {
            Code = code;
            Errors = errors;
        }

        public HttpStatusCode Code { get; set; }
        public object Errors { get; set; }
    }
}
