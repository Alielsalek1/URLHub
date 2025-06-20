public class ApiResponse
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public object? Data { get; set; } = null;
    
    public ApiResponse(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
    
    public ApiResponse(string message, int statusCode, object data)
    {
        Message = message;
        StatusCode = statusCode;
        Data = data;
    }
}