using SendGrid;
using SendGrid.Helpers.Mail;

namespace Disney.SendGrid
{
    public static class EmailSender
    {
        public static async Task SendWelcomeEmail(string email)
        {
            var apiKey = "api_key_secret";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("myEmail", "username");
            var subject = $"Welcome {email}, now you are registed in Disney Movies App.";
            var to = new EmailAddress(email, email);
            var plainTextContent = "Enjoy...";
            var htmlContent = "<strong>Enjoy...</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
