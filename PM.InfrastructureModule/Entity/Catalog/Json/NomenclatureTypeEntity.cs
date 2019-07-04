namespace PM.InfrastructureModule.Entity.Catalog.Json
{
    public class NomenclatureTypeEntity
    {
        public string nomenclature_type_name { get; set; }

        public int? nomenclature_type_form_number { get; set; }

        public string[] order_type_guid { get; set; }
    }
}