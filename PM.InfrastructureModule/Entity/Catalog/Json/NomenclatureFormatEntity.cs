namespace PM.InfrastructureModule.Entity.Catalog.Json
{
    public class NomenclatureFormatEntity
    {
        public string nomenclature_format_name { get; set; }

        public int? width { get; set; }

        public int? height { get; set; }

        public int[] nomenclature_format_form_number { get; set; }
    }
}