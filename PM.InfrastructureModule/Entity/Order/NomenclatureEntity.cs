using System;

namespace PM.InfrastructureModule.Entity.Order
{
    public class NomenclatureEntity
    {
        public string nomenclature_guid { get; set; }

        public string nomenclature_type_guid { get; set; }

        public string order_guid { get; set; }

        public string nomenclature_info { get; set; }

        public bool deleted { get; set; }

        public DateTime dtcreate { get; set; }

        public DateTime dtmodified { get; set; }
    }
}