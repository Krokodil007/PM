using System;

namespace PM.InfrastructureModule.Entity.Contact
{
    public class ContactBankAccountEntity
    {
        public string contact_bank_account_guid { get; set; }

        public string contact_guid { get; set; }

        public string contact_bank_account_info { get; set; }

        public bool deleted { get; set; }

        public DateTime dtcreate { get; set; }

        public DateTime dtmodified { get; set; }
    }
}