using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Application.Utils.Email
{
    public class EmailManger : IEmailManager
    {
        private readonly SendGridClient _clientKey;
        private readonly IConfiguration _config;
        private readonly EmailAddress _from;
        public EmailManger()
        {

        }

        public string GetResetPasswordEmailTemplate(string emailLink, string email)
        {
            string body;
            var folderName = Path.Combine("wwwroot", "Templates", "ResetPassword.html");
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (File.Exists(filepath))
                body = File.ReadAllText(filepath);
            else
                return null;

            string msgBody = body.Replace("{email_link}", emailLink).
                Replace("{email}", email);

            return msgBody;
        }
    }
}
