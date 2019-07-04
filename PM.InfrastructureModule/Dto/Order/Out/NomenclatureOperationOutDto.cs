using PM.InfrastructureModule.Dto.Order.InOut;

namespace PM.InfrastructureModule.Dto.Order.Out
{
    public class NomenclatureOperationOutDto
    {
        /// <summary>
        /// Идентификатор номенклатуры
        /// </summary>
        public string nomenclature_guid { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор работы
        /// </summary>
        public string operation_guid { get; set; } = string.Empty;

        /// <summary>
        /// Название работы
        /// </summary>
        public string operation_name { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор единицы измерения
        /// </summary>
        public string measure_guid { get; set; } = string.Empty;

        /// <summary>
        /// Название единицы измерения
        /// </summary>
        public string measure_name { get; set; } = string.Empty;

        /// <summary>
        /// Цена
        /// </summary>
        public decimal? operation_price { get; set; } = 0;

        /// <summary>
        /// Кол-во
        /// </summary>
        public decimal? operation_count { get; set; } = 0;

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal? operation_price_total { get; set; } = 0;

        /// <summary>
        /// Стоимость Src
        /// </summary>
        public decimal? operation_price_total_src { get; set; } = 0;

        public NomenclatureOperationAttribute operation_attribute { get; set; }
    }
}