using System.Collections.Generic;
using System.Threading.Tasks;
using PM.InfrastructureModule.Dto.Order.In;
using PM.InfrastructureModule.Dto.Order.Out;

namespace PM.InfrastructureModule.Domain.Services.Order
{
    /// <summary>
    /// Заказы
    /// </summary>
    public interface IOrderDomain
    {
        /// <summary>
        /// Order Get
        /// </summary>
        Task<IEnumerable<OrderDto>> OrderGet(string orderGuid, string serviceGroupGuid);

        /// <summary>
        /// Order Upd
        /// </summary>
        Task<OrderDto> OrderUpd(OrderDto dtoItem, string userGuid, string serviceGroupGuid);

        /// <summary>
        /// Nomenclature Get
        /// </summary>
        Task<IEnumerable<NomenclatureOutDto>> NomenclatureGet(string nomenclatureGuid, string orderGuid,
            string serviceGroupGuid);

        /// <summary>
        /// Nomenclature Upd
        /// </summary>
        Task<List<NomenclatureOutDto>> NomenclatureUpd(List<NomenclatureOutDto> dtoItemList, string userGuid,
            string serviceGroupGuid);

        /// <summary>
        /// Nomenclature Del
        /// </summary>
        Task NomenclatureDel(List<string> nomenclatureGuidList, string serviceGroupGuid);

        /// <summary>
        /// Расчет номенклатуры
        /// </summary>
        Task<NomenclatureOutDto> NomenclatureCalculateForm1(NomenclatureForm1InDto nomenclatureItem, string serviceGroupGuid);

        /// <summary>
        /// Расчет номенклатуры
        /// </summary>
        Task<NomenclatureOutDto> NomenclatureCalculateForm3(NomenclatureForm3InDto nomenclatureItem, string serviceGroupGuid);

        /// <summary>
        /// Расчет скидки/наценки по заказу
        /// </summary>
        OrderFullDto OrderDiscountMarkupCalculate(OrderFullDto order);
    }
}