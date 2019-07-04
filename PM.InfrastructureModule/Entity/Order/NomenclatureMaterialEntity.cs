using System;

namespace PM.InfrastructureModule.Entity.Order
{
    public class NomenclatureMaterialEntity
    {
        public string nomenclature_material_guid { get; set; }

        public string service_group_guid { get; set; }

        public string user_guid { get; set; }

        public string nomenclature_guid { get; set; }

        public string material_guid { get; set; }

        public decimal? material_count { get; set; }

        public decimal? material_price { get; set; }

        public decimal? material_price_total { get; set; }

        public decimal? material_price_total_src { get; set; }

        public string material_attribute { get; set; }

        public bool deleted { get; set; }

        public DateTime dtcreate { get; set; }

        public DateTime dtmodified { get; set; }
    }
}