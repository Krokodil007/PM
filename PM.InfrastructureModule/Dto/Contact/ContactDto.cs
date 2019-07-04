namespace PM.InfrastructureModule.Dto.Contact
{
    public class ContactDto
    {
        public string contact_guid { get; set; }

        public string contact_bank_account_guid { get; set; }

        /// <summary>
        /// Тип контрагента
        /// </summary>
        public string contact_type_guid { get; set; }

        /// <summary>
        /// Форма собственности
        /// </summary>
        public string ownership_type_guid { get; set; }

        /// <summary>
        /// Наименование для поиска
        /// </summary>
        public string contact_name { get; set; }

        /// <summary>
        /// Полное юридическое наименование
        /// </summary>
        public string full_name { get; set; }

        /// <summary>
        /// Основной телефон
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// Дополнительный телефон
        /// </summary>
        public string phone_additional { get; set; }

        /// <summary>
        /// Основной факс
        /// </summary>
        public string fax { get; set; }

        /// <summary>
        /// Дополнительный факс
        /// </summary>
        public string fax_additional { get; set; }

        /// <summary>
        /// Сайт компании
        /// </summary>
        public string website { get; set; }

        /// <summary>
        /// Корпоративный Email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public string contract_number { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public string contract_date { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string legal_address { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// Признак подтверждения
        /// </summary>
        public bool approved { get; set; }
    }
}