namespace email_service.Models
{
    public class EmailConfig
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public int Timeout { get; set; } = 60000; 
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
  
}
