using System;
using System.Collections.Generic;

namespace PM.InfrastructureModule.Dto.Order.Out
{
    public class OrderFullDto
    {
        public string order_guid { get; set; }

        public string user_fullname { get; set; }

        public string contact_guid { get; set; }

        public string contact_name { get; set; }

        public string order_type_guid { get; set; }

        public string order_type_name { get; set; }

        public string order_status_guid { get; set; }

        public string order_status_name { get; set; }

        public int client_number { get; set; }

        public string order_name { get; set; }

        public string description { get; set; }

        public long order_count { get; set; }
        
        public bool urgent { get; set; }

        public DateTime? date_complete { get; set; }

        public DateTime? date_confirm { get; set; }

        public DateTime? date_production { get; set; }

        public bool approved { get; set; }

        public DateTime? dtcreate { get; set; }

        /// <summary>
        /// Скидка
        /// </summary>
        public double order_discount { get; set; }

        /// <summary>
        /// Наценка
        /// </summary>
        public double order_markup { get; set; }

        /// <summary>
        /// Стоимость без НДС
        /// </summary>
        public decimal? order_price_total { get; set; }

        /// <summary>
        /// Стоимость с НДС
        /// </summary>
        public decimal? order_price_total_vat { get; set; }

        /// <summary>
        /// Значение НДС
        /// </summary>
        public decimal? order_vat_value { get; set; }

        /// <summary>
        /// Сумма НДС
        /// </summary>
        public decimal? order_vat_amount { get; set; }

        public List<NomenclatureOutDto> nomenclature { get; set; }
    }
}