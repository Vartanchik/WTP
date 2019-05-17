namespace WTP.WebAPI.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Info { get; set; }

        public ResponseModel()
        { }

        public ResponseModel(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ResponseModel(int statusCode, string message, string info)
        {
            StatusCode = statusCode;
            Message = message;
            Info = info;
        }
    }
}
