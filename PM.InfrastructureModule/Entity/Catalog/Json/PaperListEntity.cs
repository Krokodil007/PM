namespace PM.InfrastructureModule.Entity.Catalog.Json
{
    public class PaperListEntity
    {
        public string material_type_guid { get; set; }

        public string paper_list_name { get; set; }

        public string vendor_code { get; set; }

        public string characteristic { get; set; }

        public string paper_list_type_guid { get; set; }

        public int? height { get; set; }

        public int? width { get; set; }

        public int? thickness { get; set; }

        public int? density { get; set; }

        public int? quantity { get; set; }

        public decimal? price { get; set; }

        public string measure_unit_guid { get; set; }

        public string manufacturer_guid { get; set; }
    }
}