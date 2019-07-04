namespace PM.InfrastructureModule.Entity.Equipment
{
    public class EquipmentColorfulnessEditionPrice
    {

        public string equipment_colorfulness_edition_price_guid { get; set; }

        public string equipment_guid { get; set; }

        public long edition_min { get; set; }

        public long edition_max { get; set; }

        public decimal price1 { get; set; }

        public decimal price2 { get; set; }

        public decimal price3 { get; set; }

        public decimal price4 { get; set; }

        public decimal price5 { get; set; }

        public decimal price6 { get; set; }

        public decimal price7 { get; set; }

        public decimal? GetColorPrice(int color)
        {
            decimal? price;
            switch (color)
            {
                case 1:
                    price = price1;
                    break;
                case 2:
                    price = price2;
                    break;
                case 3:
                    price = price3;
                    break;
                case 4:
                    price = price4;
                    break;
                case 5:
                    price = price5;
                    break;
                case 6:
                    price = price6;
                    break;
                case 7:
                    price = price7;
                    break;
                default:
                    price = 0;
                    break;
            }

            return price;
        }
    }
}