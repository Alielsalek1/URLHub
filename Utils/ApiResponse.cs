public class ApiResponse
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public object Data { get; set; }
    
    public ApiResponse(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        Data = null;
    }
    
    public ApiResponse(object data, string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        Data = data;
    }
}