using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using JetBrains.Annotations;
using PM.InfrastructureModule.Common.Mail;
using PM.InfrastructureModule.DataCore;
using PM.InfrastructureModule.Entity.Equipment;

namespace PM.InfrastructureModule.Repository.StaticQuery.Equipment
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    public class EquipmentInfo
    {
        /// <summary>
        /// Equipment Get
        /// </summary>
        public static async Task<IEnumerable<EquipmentEntity>> EquipmentGet(string equipmentGuid,
            string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<EquipmentEntity>("pm_equipment_get", new
                    {
                        equipment_guid_in = equipmentGuid,
                        service_group_guid_in = serviceGroupGuid
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
                return null;
            }
        }

        /// <summary>
        /// Equipment Upd
        /// </summary>
        public static async Task<EquipmentEntity> EquipmentUpd(EquipmentEntity item, string userGuid,
            string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<EquipmentEntity>(
                    "pm_equipment_upd", new
                    {
                        equipment_guid_in = item.equipment_guid,
                        service_group_guid_in = serviceGroupGuid,
                        user_guid_in = userGuid,
                        equipment_type_guid_in = item.equipment_type_guid,
                        equipment_info_in = item.equipment_info
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
                return null;
            }
        }

        /// <summary>
        /// Equipment Del
        /// </summary>
        public static async Task EquipmentDel(string equipmentGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_equipment_del", new
                    {
                        equipment_guid_in = equipmentGuid,
                        service_group_guid_in = serviceGroupGuid
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
            }
        }

        /// <summary>
        /// EquipmentAdjustmentEditionPrice Get
        /// </summary>
        public static async Task<IEnumerable<EquipmentAdjustmentEditionPrice>> EquipmentAdjustmentEditionPriceGet(
            string equipmentGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<EquipmentAdjustmentEditionPrice>(
                    "pm_equipment_adjustment_edition_price_get", new
                    {
                        equipment_guid_in = equipmentGuid,
                        service_group_guid_in = serviceGroupGuid
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
                return null;
            }
        }

        /// <summary>
        /// EquipmentAdjustmentGroupEdition Get
        /// </summary>
        public static async Task<IEnumerable<EquipmentAdjustmentGroupEdition>>
            EquipmentAdjustmentGroupEditionGet(string equipmentGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<EquipmentAdjustmentGroupEdition>(
                    "pm_equipment_adjustment_group_edition_get", new
                    {
                        equipment_guid_in = equipmentGuid,
                        service_group_guid_in = serviceGroupGuid
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
                return null;
            }
        }

        /// <summary>
        /// EquipmentAdjustmentGroup Get
        /// </summary>
        public static async Task<IEnumerable<EquipmentAdjustmentGroup>> EquipmentAdjustmentGroupGet(
            string equipmentGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<EquipmentAdjustmentGroup>(
                    "pm_equipment_adjustment_group_get", new
                    {
                        equipment_guid_in = equipmentGuid,
                        service_group_guid_in = serviceGroupGuid
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
                return null;
            }
        }

        /// <summary>
        /// EquipmentColorfulnessEditionPrice Get
        /// </summary>
        public static async Task<IEnumerable<EquipmentColorfulnessEditionPrice>> EquipmentColorfulnessEditionPriceGet(
            string equipmentGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<EquipmentColorfulnessEditionPrice>(
                    "pm_equipment_colorfulness_edition_price_get", new
                    {
                        equipment_guid_in = equipmentGuid,
                        service_group_guid_in = serviceGroupGuid
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
                return null;
            }
        }
    }
}