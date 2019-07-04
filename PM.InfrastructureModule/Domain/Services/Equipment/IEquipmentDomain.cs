using System.Collections.Generic;
using System.Threading.Tasks;
using PM.InfrastructureModule.Dto.Equipment;

namespace PM.InfrastructureModule.Domain.Services.Equipment
{
    /// <summary>
    /// Оборудование
    /// </summary>
    public interface IEquipmentDomain
    {
        /// <summary>
        /// Equipment Get
        /// </summary>
        Task<IEnumerable<EquipmentDto>> EquipmentGet(string equipmentGuid, string serviceGroupGuid);

        /// <summary>
        /// Equipment Upd
        /// </summary>
        Task<EquipmentDto> EquipmentUpd(EquipmentDto dtoItem, string userGuid, string serviceGroupGuid);
       
    }
}