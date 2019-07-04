using PM.InfrastructureModule.Dto.Order.InOut;

namespace PM.InfrastructureModule.Dto.Order.Out
{
    public class NomenclatureMaterialOutDto
    {
        /// <summary>
        /// Идентификатор номенклатуры
        /// </summary>
        public string nomenclature_guid { get; set; } = string.Empty;

        /// <summary>
        /// Тип материала
        /// </summary>
        public string material_type_guid { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор материала
        /// </summary>
        public string material_guid { get; set; } = string.Empty;

        /// <summary>
        /// Название материала
        /// </summary>
        public string material_name { get; set; } = string.Empty;

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
        public decimal? material_price { get; set; } = 0;

        /// <summary>
        /// Кол-во
        /// </summary>
        public decimal? material_count { get; set; } = 0;

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal? material_price_total { get; set; } = 0;

        /// <summary>
        /// Стоимость Src
        /// </summary>
        public decimal? material_price_total_src { get; set; } = 0;

        public NomenclatureMaterialAttribute material_attribute { get; set; }
    }
}