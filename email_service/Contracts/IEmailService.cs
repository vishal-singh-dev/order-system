using email_service.Models;

namespace email_service.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailModel email);
        Task SendWithAttachmentAsync(EmailModel email, List<IFormFile> files);

    }
}
