namespace WTP.BLL.Dto.AppUser
{
    public class ChangePasswordDto
    {
        public ChangePasswordDto(int userId, string currentPassword, string newPassword)
        {
            UserId = userId;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }

        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
