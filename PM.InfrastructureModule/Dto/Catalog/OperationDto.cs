namespace PM.InfrastructureModule.Dto.Catalog
{
    public class OperationDto
    {
        public string object_type_guid { get; set; }

        public string operation_guid { get; set; }

        public string operation_name { get; set; }

        public string measure_unit_guid { get; set; }

        public string measure_unit_name { get; set; }

        public decimal? operation_price { get; set; }

        public int? operation_complexity { get; set; }
    }
}