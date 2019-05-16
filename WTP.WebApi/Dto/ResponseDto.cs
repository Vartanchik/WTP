namespace WTP.WebAPI.Dto
{
    public class ResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Info { get; set; }

        public ResponseDto()
        { }

        public ResponseDto(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ResponseDto(int statusCode, string message, string info)
        {
            StatusCode = statusCode;
            Message = message;
            Info = info;
        }
    }
}
