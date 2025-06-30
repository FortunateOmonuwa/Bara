namespace SharedModule.Utils
{
    public class ResponseDetail<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; } = default;
        public int StatusCode { get; set; }
        public string? Error { get; set; }

        public static ResponseDetail<T> Successful(T data, string message = "Operation completed successfully", int statusCode = 200)
        {
            return new ResponseDetail<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static ResponseDetail<T> Failed(string message = "Operation Failed", int statusCode = 400, string? error = null)
        {
            return new ResponseDetail<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode,
                Error = error
            };
        }
    }
}
