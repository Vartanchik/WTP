namespace WTP.BLL.Services
{
    public class ServiceResult
    {
        public ServiceResult(bool succeded = true)
        {
            Succeeded = succeded;
        }

        public ServiceResult(string error, bool succeded = false)
        {
            Succeeded = succeded;
            Error = error;
        }

        public bool Succeeded { get; set; }
        public string Error { get; set; }
    }
}
