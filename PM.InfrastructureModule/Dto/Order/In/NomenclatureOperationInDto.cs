using PM.InfrastructureModule.Dto.Order.InOut;

namespace PM.InfrastructureModule.Dto.Order.In
{
    public class NomenclatureOperationInDto
    {
        /// <summary>
        /// Идентификатор работы
        /// </summary>
        public string operation_guid { get; set; } = string.Empty;

        /// <summary>
        /// Кол-во
        /// </summary>
        public decimal? operation_count { get; set; } = 0;

        public NomenclatureOperationAttribute operation_attribute { get; set; }
    }
}