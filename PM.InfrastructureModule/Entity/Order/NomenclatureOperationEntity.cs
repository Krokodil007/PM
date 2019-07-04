using System;

namespace PM.InfrastructureModule.Entity.Order
{
    public class NomenclatureOperationEntity
    {
        public string nomenclature_operation_guid { get; set; }

        public string service_group_guid { get; set; }

        public string user_guid { get; set; }

        public string nomenclature_guid { get; set; }

        public string operation_guid { get; set; }

        public decimal? operation_count { get; set; }

        public decimal? operation_price { get; set; }

        public decimal? operation_price_total { get; set; }

        public decimal? operation_price_total_src { get; set; }

        public string operation_attribute { get; set; }

        public DateTime dtcreate { get; set; }

        public DateTime dtmodified { get; set; }
    }
}