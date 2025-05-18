using email_service.Contracts;
using email_service.Models;
using email_service.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace email_service.Controllers
{
    [Route("ap/email/")]
    public class emailController : Controller
    {
        public readonly IEmailService _emailService;
        public emailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("sendemail")]
        public async Task<ActionResult> SendEmail(EmailModel email)
        {
            await _emailService.SendEmailAsync(email);
            return Ok(new{message="Email Sent" });
        }
        [HttpPost("sendemail-attachment")]
        public async Task<ActionResult> SendEmailAttachment(EmailModel email)
        {
            List<IFormFile> files=Request.Form.Files.ToList();
            if(files!=null && files.Count > 0)
            {
                await _emailService.SendWithAttachmentAsync(email, files);
                return Ok(new { message = "Email Sent" });
            }
          return BadRequest(new { message = "No files found" });
        }
    }
}
