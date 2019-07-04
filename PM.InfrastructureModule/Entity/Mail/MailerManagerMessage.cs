using System.Collections.Generic;

namespace PM.InfrastructureModule.Entity.Mail
{
    /// <summary>
    /// MailSender Message
    /// </summary>
    public class MailerManagerMessage
    {
        /// <summary>
        /// Subject
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// From
        /// </summary>
        public string from_name { get; set; }

        /// <summary>
        /// Body
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// RecipientList
        /// </summary>
        public List<MailerManagerAddress> recipient_list { get; set; }
    }
}
