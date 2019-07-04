using System;

namespace PM.InfrastructureModule.Dto.Order.Out
{
    public class OrderDto
    {
        public string order_guid { get; set; } = string.Empty;

        public string user_fullname { get; set; } = string.Empty;

        public string contact_guid { get; set; } = string.Empty;

        public string contact_name { get; set; } = string.Empty;

        public string order_type_guid { get; set; } = string.Empty;

        public string order_type_name { get; set; } = string.Empty;

        public string order_status_guid { get; set; } = string.Empty;

        public string order_status_name { get; set; } = string.Empty;

        public int client_number { get; set; } = 0;

        public string order_name { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public long order_count { get; set; } = 0;

        public bool urgent { get; set; }

        public DateTime? date_complete { get; set; } = DateTime.MinValue;

        public DateTime? date_confirm { get; set; } = DateTime.MinValue;

        public DateTime? date_production { get; set; } = DateTime.MinValue;

        public bool approved { get; set; }

        public DateTime? dtcreate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Стоимость без НДС
        /// </summary>
        public decimal? order_price_total { get; set; } = 0;

        /// <summary>
        /// Стоимость с НДС
        /// </summary>
        public decimal? order_price_total_vat { get; set; } = 0;

        /// <summary>
        /// Значение НДС
        /// </summary>
        public decimal? order_vat_value { get; set; } = 0;

        /// <summary>
        /// Сумма НДС
        /// </summary>
        public decimal? order_vat_amount { get; set; } = 0;

    }
}