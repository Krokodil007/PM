namespace PM.InfrastructureModule.Entity.Equipment
{
    public class EquipmentAdjustmentGroupEdition
    {

        public string equipment_adjustment_group_edition_guid { get; set; }

        public string equipment_guid { get; set; }

        public string paper_list_type_guid { get; set; }

        public long edition { get; set; }

        public decimal group1 { get; set; }

        public decimal group2 { get; set; }

        public decimal group3 { get; set; }

        public decimal GetWasteRatio(int groupId)
        {
            decimal ratio;
            switch (groupId)
            {
                case 1:
                    ratio = group1;
                    break;
                case 2:
                    ratio = group2;
                    break;
                case 3:
                    ratio = group3;
                    break;
                default:
                    ratio = 0;
                    break;
            }

            return ratio;
        }
    }
}