namespace WTP.BLL.Models.AppUser
{
    public class ChangePasswordModel
    {
        public ChangePasswordModel(int userId, string currentPassword, string newPassword)
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
