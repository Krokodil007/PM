using System;

namespace PM.InfrastructureModule.Entity.Contact
{
    public class ContactPersonEntity
    {
        public string contact_person_guid { get; set; }

        public string contact_guid { get; set; }

        public string contact_person_info { get; set; }

        public bool deleted { get; set; }

        public DateTime dtcreate { get; set; }

        public DateTime dtmodified { get; set; }
    }
}