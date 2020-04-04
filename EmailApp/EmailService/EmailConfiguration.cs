using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService
{
    public class EmailConfiguration
    {
        /// <summary>
        /// Mail
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// mail sserver
        /// </summary>
        public string SmtpServer { get; set; }
        /// <summary>
        /// Cổng mail
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Tài khoản mail
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Mật khẩu mail
        /// </summary>
        public string Password { get; set; }
    }
}
