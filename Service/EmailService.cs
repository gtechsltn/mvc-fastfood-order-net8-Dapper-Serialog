

using System.Net.Mail;
using System.Net;
using FastFood.Service.Interface;
namespace FastFood.Service
{
    public class EmailService : IEmailService
    {
        private IWebHostEnvironment _environment;
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IWebHostEnvironment environment, IConfiguration config, ILogger<EmailService> logger)
        {
            _environment = environment;
            _config = config;
            _logger = logger;
        }

        public async Task<string> GetHTMLTemplate(string subject, string body)
        {
            string EmailHtmlTemplate = "";
            try
            {
                // Construct the path to the template file inside the Helper directory
                string templatePath = Path.Combine(_environment.ContentRootPath, "Helper", "EmailTemplate.txt");

                // Read the content of the template file
                EmailHtmlTemplate = await File.ReadAllTextAsync(templatePath);
                EmailHtmlTemplate = EmailHtmlTemplate.Replace("***SUBJECT***", subject);
                EmailHtmlTemplate = EmailHtmlTemplate.Replace("***CONTENT***", body);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Unable to read the Email HTML Template");
            }
            return EmailHtmlTemplate;
        }

        public async Task SendMail(string subject, string body, string receiver)
        {
           
            try
            {
                string? senderEmail = _config.GetValue<string>("Email:SenderEmail");
                string? senderPassword = _config.GetValue<string>("Email:SenderPassword");

                if (senderEmail != null && senderPassword != null)
                {
                    MailMessage mm = new MailMessage(senderEmail, receiver);
                    mm.IsBodyHtml = true;
                    mm.Subject = subject;
                    mm.Body = await GetHTMLTemplate(subject, body);

                    // I used gmail account for the smtp: provide your email smtp here
                    var client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential(senderEmail, senderPassword),
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    client.Send(mm);                    
                }
                else _logger.LogError("Unable to read the Email Configuration in appsettings");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to send message to {receiver}");
            }       
        }
    }
}
