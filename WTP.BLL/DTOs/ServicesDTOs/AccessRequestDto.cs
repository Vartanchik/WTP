namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class AccessRequestDto
    {
        public string GrantType { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public string Password { get; set; }
    }
}
