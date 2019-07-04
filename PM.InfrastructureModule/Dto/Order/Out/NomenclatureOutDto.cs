using System;
using System.Collections.Generic;

namespace PM.InfrastructureModule.Dto.Order.Out
{
    public class NomenclatureOutDto
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
        public double order_discount { get; set; }

        /// <summary>
        /// Наценка
        /// </summary>
        public double order_markup { get; set; }

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
        public int nomenclature_count { get; set; }

        /// <summary>
        /// Именованный формат
        /// </summary>
        public string format_guid { get; set; } = string.Empty;

        /// <summary>
        /// Тип обложки
        /// </summary>
        public string cover_type_guid { get; set; } = string.Empty;

        /// <summary>
        /// Формат ширина
        /// </summary>
        public int format_width { get; set; }

        /// <summary>
        /// Формат высота
        /// </summary>
        public int format_height { get; set; }

        /// <summary>
        /// Id Корешка
        /// </summary>
        public string koreshok_type_guid { get; set; } = string.Empty;

        /// <summary>
        /// Форматы сторонок Ширина
        /// </summary>
        public int storonki_a_width { get; set; }

        /// <summary>
        /// Форматы сторонок Высота
        /// </summary>
        public int storonki_b_height { get; set; }

        /// <summary>
        /// Красочность 1
        /// </summary>
        public int colorfulness1 { get; set; }

        /// <summary>
        /// Красочность 2
        /// </summary>
        public int colorfulness2 { get; set; }

        /// <summary>
        /// Оборудование
        /// </summary>
        public string equipment_guid { get; set; }

        /// <summary>
        /// Рабочий формат ширина
        /// </summary>
        public int equipment_print_width { get; set; }

        /// <summary>
        /// Рабочий формат высота
        /// </summary>
        public int equipment_print_height { get; set; }

        /// <summary>
        /// Группа сложности
        /// </summary>
        public int complexity { get; set; }

        /// <summary>
        /// Количество полос
        /// </summary>
        public int polosi_count { get; set; }

        /// <summary>
        /// Отстав корешка
        /// </summary>
        public int otstav_koreshka { get; set; }

        /// <summary>
        /// Расстав корешка
        /// </summary>
        public int rasstav_koreshka { get; set; }

        /// <summary>
        /// Листы
        /// </summary>
        public int list_count { get; set; }

        /// <summary>
        /// Листопрогон
        /// </summary>
        public int nomenclature_print_count { get; set; }

        /// <summary>
        /// Кол-во изделий на одном листе
        /// </summary>
        public int nomenclature_per_list_count { get; set; }

        /// <summary>
        /// Резка до печати
        /// </summary>
        public int proportion_after_print { get; set; }

        /// <summary>
        /// Формат листа после резки
        /// </summary>
        public int work_format_width { get; set; }

        /// <summary>
        /// Формат листа после резки
        /// </summary>
        public int work_format_height { get; set; }

        /// <summary>
        /// Формат печатного поля
        /// </summary>
        public int print_field_width { get; set; }

        /// <summary>
        /// Формат печатного поля
        /// </summary>
        public int print_field_height { get; set; }

        /// <summary>
        /// Кол-во форм
        /// </summary>
        public int form_count { get; set; }

        /// <summary>
        /// Приладки
        /// </summary>
        public int priladki_count { get; set; }

        /// <summary>
        /// Стоимость без НДС
        /// </summary>
        public decimal price_total { get; set; }

        /// <summary>
        /// Стоимость без НДС
        /// </summary>
        public decimal price_total_src { get; set; }

        /// <summary>
        /// Стоимость с НДС
        /// </summary>
        public decimal price_total_vat { get; set; }

        /// <summary>
        /// Стоимость с НДС SRC
        /// </summary>
        public decimal price_total_vat_src { get; set; }

        /// <summary>
        /// Сумма НДС
        /// </summary>
        public decimal vat_amount { get; set; }

        /// <summary>
        /// Значение НДС
        /// </summary>
        public decimal vat_value { get; set; }

        /// <summary>
        /// Кол-во полос
        /// </summary>
        public int band_count { get; set; } = 0;

        /// <summary>
        /// Тетради
        /// </summary>
        public int notebook_count { get; set; } = 0;

        public bool nomenclature_approved { get; set; }

        public List<string> nomenclature_message { get; set; }

        public List<NomenclatureMaterialOutDto> nomenclature_material { get; set; }

        public List<NomenclatureOperationOutDto> nomenclature_operation { get; set; }

    }
}