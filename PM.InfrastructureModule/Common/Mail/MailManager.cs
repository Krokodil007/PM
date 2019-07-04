using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using PM.InfrastructureModule.Common.App;
using PM.InfrastructureModule.Entity.Mail;

namespace PM.InfrastructureModule.Common.Mail
{
    /// <summary>
    /// Отправка сообщений
    /// </summary>
    [UsedImplicitly]
    public static class MailerManager
    {
        private static readonly IConfigurationRoot Config = AppSettingBuilder.GetAppSettings();

        /// <summary>
        /// Отправка почтовых сообщений
        /// </summary>
        public static void MailSender(string subject, List<MailerManagerAddress> recipientList, string fromName = null,
            string body = null)
        {
            var messageList = new List<MailerManagerMessage>
            {
                new MailerManagerMessage
                {
                    subject = subject,
                    body = body,
                    from_name = fromName,
                    recipient_list = recipientList
                }
            };

            BuildMessage(messageList);
        }

        /// <summary>
        /// Отправка почтовых сообщений
        /// </summary>
        public static async Task MailSenderAsync(string subject, List<MailerManagerAddress> recipientList,
            string fromName = null,
            string body = null)
        {
            var messageList = new List<MailerManagerMessage>
            {
                new MailerManagerMessage
                {
                    subject = subject,
                    body = body,
                    from_name = fromName,
                    recipient_list = recipientList
                }
            };

            await BuildMessageAsync(messageList);
        }

        /// <summary>
        /// Отправка почтовых сообщений
        /// </summary>
        public static async Task MailSenderAsync(List<MailerManagerMessage> messageList)
        {
            await BuildMessageAsync(messageList);
        }

        /// <summary>
        /// Создание сообщения
        /// </summary>
        private static async Task BuildMessageAsync(List<MailerManagerMessage> messageList)
        {
            using (var client = new SmtpClient(Config["FluentEmail:server"],
                Convert.ToInt32(Config["FluentEmail:serverport"]))
            {
                Credentials = new NetworkCredential(Config["FluentEmail:login"], Config["FluentEmail:password"]),
                EnableSsl = true
            })
            {
                Email.DefaultSender = new SmtpSender(client);
                foreach (var m in messageList)
                {
                    await Email
                        .From(Config["FluentEmail:fromaddress"],
                            string.IsNullOrEmpty(m.from_name) ? Config["FluentEmail:fromname"] : m.from_name)
                        .To(m.recipient_list.Select(c => new Address(c.email_address, c.name)).ToList())
                        .Subject(m.subject)
                        .Body(m.body, isHtml: true)
                        .SendAsync();
                }
            }
        }

        /// <summary>
        /// Создание сообщения
        /// </summary>
        private static bool BuildMessage(List<MailerManagerMessage> messageList)
        {
            try
            {
                using (var client = new SmtpClient(Config["FluentEmail:server"],
                    Convert.ToInt32(Config["FluentEmail:serverport"]))
                {
                    Credentials = new NetworkCredential(Config["FluentEmail:login"], Config["FluentEmail:password"]),
                    EnableSsl = true
                })
                {
                    Email.DefaultSender = new SmtpSender(client);
                    foreach (var m in messageList)
                    {
                        var email = Email
                            .From(Config["FluentEmail:fromaddress"],
                                string.IsNullOrEmpty(m.from_name) ? Config["FluentEmail:fromname"] : m.from_name)
                            .To(m.recipient_list.Select(c => new Address(c.email_address, c.name)).ToList())
                            .Subject(m.subject)
                            .Body(m.body, isHtml: true)
                            .Send();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}