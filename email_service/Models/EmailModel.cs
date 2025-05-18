namespace email_service.Models
{
    public class EmailModel
    {
        public string Subject{ get; set; }
        public string Body{ get; set; }
        public string From { get; set; }
        public List<string> Recipients { get; set; }
        public string SenderName{ get; set; }
        public string SenderMobile{ get; set; }
        public bool isHtml { get; set; }
    }
}
