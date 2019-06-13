namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class ChangeUserPasswordDto
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
