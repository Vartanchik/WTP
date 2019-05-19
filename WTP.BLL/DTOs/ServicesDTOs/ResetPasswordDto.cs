namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class ResetPasswordDto
    {
        public ResetPasswordDto(int id, string token, string newPassword)
        {
            Id = id;
            Token = token;
            NewPassword = newPassword;
        }

        public int Id { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
