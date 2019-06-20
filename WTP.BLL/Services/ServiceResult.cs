namespace WTP.BLL.Services
{
    public class ServiceResult
    {
        /// <summary>
        /// Successful result
        /// </summary>
        public ServiceResult(bool succeded = true)
        {
            Succeeded = succeded;
        }

        /// <summary>
        /// Unsuccessful result
        /// </summary>
        /// <param name="error"></param>
        public ServiceResult(string error, bool succeded = false)
        {
            Succeeded = succeded;
            Error = error;
        }

        public bool Succeeded { get; set; }
        public string Error { get; set; }
    }
}
