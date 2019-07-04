namespace PM.InfrastructureModule.Dto.Equipment
{
    public class EquipmentDto
    {
        public string equipment_guid { get; set; }

        public string equipment_type_guid { get; set; }

        public string equipment_name { get; set; }

        public int? max_width { get; set; }

        public int? max_height { get; set; }

        public int? min_width { get; set; }

        public int? min_height { get; set; }

        public int? colorfulness { get; set; }

        public int? priladka { get; set; }

        public bool digital { get; set; }

        public bool two_sided_print { get; set; }

        public int? top_indent { get; set; }

        public int? bottom_indent { get; set; }

        public int? left_indent { get; set; }

        public int? right_indent { get; set; }
    }
}