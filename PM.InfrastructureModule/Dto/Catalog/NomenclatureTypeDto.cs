namespace PM.InfrastructureModule.Dto.Catalog
{
    public class NomenclatureTypeDto
    {
        public string object_type_guid { get; set; }

        public string nomenclature_type_guid { get; set; }

        public string nomenclature_type_name { get; set; }

        public int? nomenclature_type_form_number { get; set; }

        public string[] order_type_guid { get; set; }
    }
}