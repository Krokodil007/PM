namespace PM.InfrastructureModule.Entity.Equipment
{
    public class EquipmentAdjustmentEditionPrice
    {
        public string equipment_adjustment_edition_price_guid { get; set; }

        public string equipment_guid { get; set; }

        public long edition_min { get; set; }

        public long edition_max { get; set; }

        public decimal? price { get; set; }
    }
}