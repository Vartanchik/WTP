namespace WTP.BLL.Models.AppUserModels
{
    public class ResetPasswordModel
    {
        public ResetPasswordModel(int id, string token, string newPassword)
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
