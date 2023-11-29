using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net;
using static System.Net.WebRequestMethods;

namespace Auth.Infrastructure.Services.Implementations
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SendOtpSmsAsync(string phone, string otp)
        {
            var access = _configuration["SmsSpeed:AccessToken"];

            var auToken = _configuration["SmsSpeed:SenderId"];

            Uri root = new Uri(_configuration["SmsSpeed:ApiEndpoint"]);

            string content = $"Your Otp is: {otp}. Please don't share this to anyone";

            NetworkCredential myCreds = new NetworkCredential(access, ":x");

            WebClient client = new WebClient();

            client.Credentials = myCreds;

            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            var builder = "{\"to\":[\"" + phone + "\"], \"content\": \""
                + content
                + "\", \"type\":" + 5
                + ", \"sender\": \""
                + auToken + "\"}";

            string json = builder.ToString();

            client.UploadStringAsync(root, json);
        }

        public async Task SendPasswordSmsAsync(string phone, string password)
        {
            var access = _configuration["SmsSpeed:AccessToken"];

            var auToken = _configuration["SmsSpeed:SenderId"];

            Uri root = new Uri(_configuration["SmsSpeed:ApiEndpoint"]);

            string content = $"Your Password is: {password}. Please don't share this to anyone";

            NetworkCredential myCreds = new NetworkCredential(access, ":x");

            WebClient client = new WebClient();

            client.Credentials = myCreds;

            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            var builder = "{\"to\":[\"" + phone + "\"], \"content\": \""
                + content
                + "\", \"type\":" + 5
                + ", \"sender\": \""
                + auToken + "\"}";

            string json = builder.ToString();

            client.UploadStringAsync(root, json);
        }

        public async Task SendPasswordEmail(string email, string password)
        {
            var result = await SendEmailAsync(new EmailViewModel()
            {

                To = email,
                Subject = $"Mật Khẩu Tài Khoản Yolo",
                Text =
                       $"<h3 style=\"color: #2856a3;\">Thông tin mật khẩu người dùng: </h3>" +
                        $"<br>" +
                       $"<h3>Mật Khẩu: {password} </h3> ",

            });

        }

        public async Task SendOtpEmail(string email, string otp)
        {
            var result = await SendEmailAsync(new EmailViewModel()
            {

                To = email,
                Subject = $"OTP Tài Khoản Yolo",
                Text =
                       $"<h3 style=\"color: #2856a3;\">Mã OTP của bạn là: </h3>" +
                       $"<br>" +
                       $"<h3>Password: {otp} </h3> ",

            });

        }

        public async Task<bool> SendEmailAsync(EmailViewModel email)
        {
            var result = true;

            var mimeemail = new MimeMessage();
            mimeemail.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:From"]));
            mimeemail.To.Add(MailboxAddress.Parse(email.To));
            mimeemail.Subject = email.Subject;
            var builder = new BodyBuilder();


            if (email.Attachment != null)
            {
                var stream = new MemoryStream();
                await email.Attachment.CopyToAsync(stream);
                stream.Position = 0;
                builder.Attachments.Add(email.Attachment.FileName, stream);
                // You can also add inline image resources in the email if needed.
                // For example, if your email text contains image placeholders and you want to display images inline.
                // var image = builder.LinkedResources.Add(email.ImagePath);
                // image.ContentId = MimeUtils.GenerateMessageId();
                // builder.HtmlBody = string.Format(email.Text, image.ContentId);
                builder.HtmlBody = email.Text;
            }
            else
            {
                builder.HtmlBody = email.Text;
            }

            mimeemail.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    var a = _configuration["EmailSettings:Username"]!.ToString();
                    var b = _configuration["EmailSettings:Password"]!.ToString();
                    await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"]!.ToString(), 587);
                    await client.AuthenticateAsync(_configuration["EmailSettings:Username"]!.ToString(), _configuration["EmailSettings:Password"]!.ToString());
                    await client.SendAsync(mimeemail);

                }
                catch (Exception ex)
                {
                    result = false;
                    Console.WriteLine(ex);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            };
            return result;
        }
        public class EmailViewModel
        {
            public string To { get; set; }
            public string Subject { get; set; }
            public string Text { get; set; }
            public IFormFile Attachment { get; set; }
        }
    }
}
