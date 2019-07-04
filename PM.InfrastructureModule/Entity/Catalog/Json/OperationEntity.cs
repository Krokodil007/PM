namespace PM.InfrastructureModule.Entity.Catalog.Json
{
    public class OperationEntity
    {
        public string operation_name { get; set; }

        public string measure_unit_guid { get; set; }

        public decimal? operation_price { get; set; }

        public int? operation_complexity { get; set; }
    }
}