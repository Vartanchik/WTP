namespace WTP.BLL.Dto.AppUser
{
    public class ResetPasswordDto
    {
        public ResetPasswordDto(int id, string token, string newPassword)
        {
            Id = id;
            Code = token;
            NewPassword = newPassword;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
