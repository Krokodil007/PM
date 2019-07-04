namespace PM.InfrastructureModule.Dto.Catalog
{
    public class NomenclatureFormatDto
    {
        public string object_type_guid { get; set; }

        public string nomenclature_format_guid { get; set; }

        public string nomenclature_format_name { get; set; }

        public int? width { get; set; }

        public int? height { get; set; }

        public int[] nomenclature_format_form_number { get; set; }
    }
}