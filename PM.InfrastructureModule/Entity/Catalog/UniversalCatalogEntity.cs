using System;

namespace PM.InfrastructureModule.Entity.Catalog
{
    public class UniversalCatalogEntity
    {
        public string object_guid { get; set; }

        public string object_type_guid { get; set; }

        public string service_group_guid { get; set; }

        public string user_guid { get; set; }

        public string object_info { get; set; }

        public DateTime dtcreate { get; set; }

        public DateTime dtmodified { get; set; }
    }
}