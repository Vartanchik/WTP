namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class EmailConfigDto
    {
        public EmailConfigDto(string email, string password, string host, string port)
        {
            Email = email;
            Password = password;
            Host = host;
            Port = port;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
