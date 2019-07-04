using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PM.InfrastructureModule.Entity.Mail;

namespace PM.InfrastructureModule.Common.Mail
{
    /// <summary>
    /// Send error message
    /// </summary>
    [UsedImplicitly]
    public class SendError
    {
        public static async Task SendErrorAsync(Exception ex, string customMessage = null, string service = null)
        {
            var subject = customMessage;
            var excMes = $"<h4>{ex.Message}</h4>" +
                         $"<br/><h4>Stack Trace:</h4>{ex.StackTrace}" +
                         $"<br/><h4>Inner Exception:</h4>{ex.InnerException}" +
                         $"<br/><h4>Custom message:</h4>{customMessage}" +
                         $"<br/><h4>HResult:</h4>{ex.HResult}" +
                         $"<br/><h4>Data:</h4>{ex.Data}" +
                         $"<br/><h4>Пользователь:</h4> {Environment.UserName}" +
                         $"<br/><h4>Хост: </h4>{Environment.MachineName}" +
                         $"<br/><h4>Время: </h4>{DateTime.Now}" +
                         $"<br/><h4>Точка входа: </h4>{ex.TargetSite}" +
                         $"</br><h4>Приложение: </h4>PM Service {service}.";
            var recipients = new List<MailerManagerAddress>
            {
                new MailerManagerAddress("error_info@pixmake.ru")
            };
            var from = "error_info@pixmake.ru";
            
            await MailerManager.MailSenderAsync(subject, recipients, from, excMes);
        }

        public static void SendErrorSync(Exception ex, string customMessage = null, string service = null)
        {
            var subject = customMessage;
            var excMes = $"<h4>{ex.Message}</h4>" +
                         $"<br/><h4>Stack Trace:</h4>{ex.StackTrace}" +
                         $"<br/><h4>Inner Exception:</h4>{ex.InnerException}" +
                         $"<br/><h4>Custom message:</h4>{customMessage}" +
                         $"<br/><h4>HResult:</h4>{ex.HResult}" +
                         $"<br/><h4>Data:</h4>{ex.Data}" +
                         $"<br/><h4>Пользователь:</h4> {Environment.UserName}" +
                         $"<br/><h4>Хост: </h4>{Environment.MachineName}" +
                         $"<br/><h4>Время: </h4>{DateTime.Now}" +
                         $"<br/><h4>Точка входа: </h4>{ex.TargetSite}" +
                         $"</br><h4>Приложение: </h4>PM Service {service}.";
            var recipients = new List<MailerManagerAddress>
            {
                new MailerManagerAddress("error_info@pixmake.ru")
            };
            var fromName = "ErrorInfoPixmake";

            MailerManager.MailSender(subject, recipients, fromName, excMes);
        }
    }
}
