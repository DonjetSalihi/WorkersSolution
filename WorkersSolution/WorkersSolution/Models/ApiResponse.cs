using System.Net;

namespace WorkersSolution.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ApiResponse() { }

        public ApiResponse(bool success, dynamic result, string message, HttpStatusCode httpStatusCode) 
        {  
            Success = success; 
            Result = result;
            Message = message;
            StatusCode = httpStatusCode;
        }
    }
}
