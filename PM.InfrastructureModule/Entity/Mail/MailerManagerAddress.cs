using System.ComponentModel.DataAnnotations;

namespace PM.InfrastructureModule.Entity.Mail
{
    public class MailerManagerAddress
    {
        public string name { get; set; }

        [Required, EmailAddress]
        public string email_address { get; set; }

        public MailerManagerAddress(string email_address, string name = null)
        {
            this.email_address = email_address;
            this.name = name;
        }
    }
}