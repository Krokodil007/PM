using System.Collections.Generic;

namespace PM.InfrastructureModule.Dto.Order.In
{
    public class NomenclatureForm1InDto
    {
        /// <summary>
        /// Идентификатор номенклатуры
        /// </summary>
        public string nomenclature_guid { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор типа номенклатуры
        /// </summary>
        public string nomenclature_type_guid { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public string order_guid { get; set; } = string.Empty;

        /// <summary>
        /// Скидка
        /// </summary>
        public double? order_discount { get; set; } = 0;

        /// <summary>
        /// Наценка
        /// </summary>
        public double? order_markup { get; set; } = 0;

        /// <summary>
        /// Идентификатор заказчика
        /// </summary>
        public string contact_guid { get; set; } = string.Empty;

        /// <summary>
        /// Название
        /// </summary>
        public string nomenclature_name { get; set; } = string.Empty;

        /// <summary>
        /// Тираж
        /// </summary>
        public int? nomenclature_count { get; set; } = 0;

        /// <summary>
        /// Именованный формат
        /// </summary>
        public string format_guid { get; set; } = string.Empty;

        /// <summary>
        /// Формат ширина
        /// </summary>
        public int? format_width { get; set; } = 0;

        /// <summary>
        /// Формат высота
        /// </summary>
        public int? format_height { get; set; } = 0;

        /// <summary>
        /// Красочность 1
        /// </summary>
        public int? colorfulness1 { get; set; } = 0;

        /// <summary>
        /// Красочность 2
        /// </summary>
        public int? colorfulness2 { get; set; } = 0;

        /// <summary>
        /// Оборудование
        /// </summary>
        public string equipment_guid { get; set; } = string.Empty;

        /// <summary>
        /// Группа сложности
        /// </summary>
        public int? complexity { get; set; } = 0;

        /// <summary>
        /// Приладки
        /// </summary>
        public int? priladki_count { get; set; } = 0;

        public List<NomenclatureMaterialInDto> nomenclature_material { get; set; }

        public List<NomenclatureOperationInDto> nomenclature_operation { get; set; }

    }
}