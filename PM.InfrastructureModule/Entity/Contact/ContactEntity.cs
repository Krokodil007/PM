using System;

namespace PM.InfrastructureModule.Entity.Contact
{
    public class ContactEntity
    {
        public string contact_guid { get; set; }

        public string contact_type_guid { get; set; }

        public string contact_info { get; set; }

        public bool deleted { get; set; }

        public DateTime dtcreate { get; set; }

        public DateTime dtmodified { get; set; }
    }
}