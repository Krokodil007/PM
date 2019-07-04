using PM.InfrastructureModule.Dto.Order.InOut;

namespace PM.InfrastructureModule.Dto.Order.In
{
    public class NomenclatureMaterialInDto
    {
        /// <summary>
        /// Идентификатор материала
        /// </summary>
        public string material_guid { get; set; } = string.Empty;

        /// <summary>
        /// Кол-во
        /// </summary>
        public decimal? material_count { get; set; } = 0;

        public NomenclatureMaterialAttribute material_attribute { get; set; }
    }
}