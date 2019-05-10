namespace WTP.WebAPI.ViewModels
{
    public class AccessRequestModel
    {
        public string GrantType { get; set; } //password or refresh token
        public string Email { get; set; }
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public string Password { get; set; }
    }
}
