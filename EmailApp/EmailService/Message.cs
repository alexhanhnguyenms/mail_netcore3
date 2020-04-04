using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailService
{

    public class Message
    {
        /// <summary>
        /// Danh sách mail được gửi
        /// </summary>
        public List<MailboxAddress> To { get; set; }
        /// <summary>
        /// Chủ đề mail
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Nội dung mail
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Đính kèm file gửi
        /// </summary>
        public IFormFileCollection Attachments { get; set; }
        /// <summary>
        /// gửi mail
        /// </summary>
        /// <param name="to">Tới mail cần gửi</param>
        /// <param name="subject">Tiêu đề mail</param>
        /// <param name="content">Nội dung cần gửi</param>
        /// <param name="attachments">File kèm theo</param>
        public Message(IEnumerable<string> to, string subject, string content, IFormFileCollection attachments)
        {
            To = new List<MailboxAddress>();
            //Thêm danh sách mail vào hộp mail
            To.AddRange(to.Select(x => new MailboxAddress(x)));
            //đặt tiêu đề cho mail
            Subject = subject;
            //nội dung cho mail
            Content = content;
            //file đính kèm
            Attachments = attachments;
        }
    }
}
