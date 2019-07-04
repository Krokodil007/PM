using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PM.InfrastructureModule.Common.Data;
using PM.InfrastructureModule.Domain.Services.Equipment;
using PM.InfrastructureModule.Dto.Equipment;
using PM.InfrastructureModule.Entity.Equipment;
using PM.InfrastructureModule.Repository.StaticQuery.Equipment;

namespace PM.InfrastructureModule.Domain.Equipment
{
    /// <summary>
    /// Контрагенты
    /// </summary>
    public class EquipmentDomain : IEquipmentDomain
    {
        /// <summary>
        /// Equipment Get
        /// </summary>
        public async Task<IEnumerable<EquipmentDto>> EquipmentGet(string equipmentGuid, string serviceGroupGuid)
        {
            var equipments = await EquipmentInfo.EquipmentGet(equipmentGuid, serviceGroupGuid);
            var result = equipments.Select(c => JsonDataExtensions.JsonToEntityData<EquipmentDto>(c.equipment_info))
                .ToList();
            return result;
        }

        /// <summary>
        /// Equipment Upd
        /// </summary>
        public async Task<EquipmentDto> EquipmentUpd(EquipmentDto dtoItem, string userGuid, string serviceGroupGuid)
        {
            var item = new EquipmentEntity
            {
                equipment_guid = dtoItem.equipment_guid,
                equipment_type_guid = dtoItem.equipment_type_guid,
                equipment_info = JsonDataExtensions.EntityToJsonData(dtoItem)
            };

            var equipment = await EquipmentInfo.EquipmentUpd(item, userGuid, serviceGroupGuid);
            var result = JsonDataExtensions.JsonToEntityData<EquipmentDto>(equipment.equipment_info);
            return result;
        }
    }
}