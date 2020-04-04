using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService
{
    /// <summary>
    /// Gửi maik
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// cấu hình mail
        /// </summary>
        private readonly EmailConfiguration _emailConfig;
        /// <summary>
        /// Contractor
        /// </summary>
        /// <param name="emailConfig">Cấu hình mail</param>
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        /// <summary>
        /// Gửi mail
        /// </summary>
        /// <param name="message"></param>
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }
        /// <summary>
        /// Gửi mail bất đồng bộ
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }
        /// <summary>
        /// Tạo nội dung mail
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            //nguwooi gửi
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            //người nhận: là danh sách
            emailMessage.To.AddRange(message.To);
            //tiêu đề mail
            emailMessage.Subject = message.Subject;
            //nội dung mail
            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };
            //danh sách file đính kèm
            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    //thêm file đinh kèm
                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }
            //gán nội dung mail
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        /// <summary>
        /// Gửi mail đồng bộ
        /// </summary>
        /// <param name="mailMessage"></param>
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    //kết nối sever
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    //xóa nội dung chứng thực cũ
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    //chứng thực: tài khoản và mật khẩu
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    //gửi mail
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    //hủy kết nối
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
        /// <summary>
        /// gửi mail bất đồng bộ
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    //kết nối sever
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    //xóa nội dung chứng thực cũ
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    //chứng thực: tài khoản và mật khẩu
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    //gửi mail
                    await client.SendAsync(mailMessage);
                }
                catch(Exception ex)
                {
                    int a = 1;
                    //log an error message or throw an exception, or both.
                    //throw;
                }
                finally
                {
                    //hủy kết nối
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
