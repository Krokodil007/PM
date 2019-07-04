using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using JetBrains.Annotations;
using PM.InfrastructureModule.Common.Mail;
using PM.InfrastructureModule.DataCore;
using PM.InfrastructureModule.Entity.Order;

namespace PM.InfrastructureModule.Repository.StaticQuery.Order
{
    /// <summary>
    /// Заказы
    /// </summary>
    [UsedImplicitly]
    public class OrderInfo
    {
        /// <summary>
        /// Order Get
        /// </summary>
        public static async Task<IEnumerable<OrderEntity>> OrderGet(string orderGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<OrderEntity>("pm_order_getV2", new
                    {
                        order_guid_in = orderGuid,
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
        /// Order Upd
        /// </summary>
        public static async Task<OrderEntity> OrderUpd(OrderEntity item, string userGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<OrderEntity>(
                    "pm_order_updV3", new
                    {
                        order_guid_in = item.order_guid,
                        service_group_guid_in = serviceGroupGuid,
                        user_guid_in = userGuid,
                        contact_guid_in = item.contact_guid,
                        order_type_guid_in = item.order_type_guid,
                        order_status_guid_in = item.order_status_guid,
                        order_info_in = item.order_info
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
        /// Order Upd
        /// </summary>
        public static async Task OrderApprovedUpd(string orderGuid, bool approved, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync(
                    "pm_order_approved_updV2", new
                    {
                        order_guid_in = orderGuid,
                        approved_in = approved,
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
        /// Order Del
        /// </summary>
        public static async Task OrderDel(string orderGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_order_del", new
                    {
                        order_guid_in = orderGuid,
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
        /// Nomenclature Get
        /// </summary>
        public static async Task<IEnumerable<NomenclatureEntity>> NomenclatureGet(string nomenclatureGuid,
            string orderGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<NomenclatureEntity>("pm_nomenclature_get", new
                    {
                        nomenclature_guid_in = nomenclatureGuid,
                        order_guid_in = orderGuid,
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
        /// Nomenclature Upd
        /// </summary>
        public static async Task<NomenclatureEntity> NomenclatureUpd(NomenclatureEntity item, string userGuid,
            string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<NomenclatureEntity>(
                    "pm_nomenclature_upd", new
                    {
                        nomenclature_guid_in = item.nomenclature_guid,
                        nomenclature_type_guid_in = item.nomenclature_type_guid,
                        order_guid_in = item.order_guid,
                        service_group_guid_in = serviceGroupGuid,
                        user_guid_in = userGuid,
                        nomenclature_info_in = item.nomenclature_info
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
        /// Nomenclature Del
        /// </summary>
        public static async Task NomenclatureDel(string nomenclatureGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_nomenclature_del", new
                    {
                        nomenclature_guid_in = nomenclatureGuid,
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
        /// Nomenclature Material Get
        /// </summary>
        public static async Task<IEnumerable<NomenclatureMaterialEntity>> NomenclatureMaterialGet(
            string nomenclatureMaterialGuid, string nomenclatureGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<NomenclatureMaterialEntity>(
                    "pm_nomenclature_material_get", new
                    {
                        nomenclature_material_guid_in = nomenclatureMaterialGuid,
                        nomenclature_guid_in = nomenclatureGuid,
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
        /// Nomenclature Material Upd
        /// </summary>
        public static async Task<NomenclatureMaterialEntity> NomenclatureMaterialUpd(NomenclatureMaterialEntity item,
            string userGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<NomenclatureMaterialEntity>(
                    "pm_nomenclature_material_upd", new
                    {
                        nomenclature_material_guid_in = item.nomenclature_material_guid,
                        nomenclature_guid_in = item.nomenclature_guid,
                        service_group_guid_in = serviceGroupGuid,
                        user_guid_in = userGuid,
                        material_guid_in = item.material_guid,
                        material_count_in = item.material_count,
                        material_price_in = item.material_price,
                        material_price_total_in = item.material_price_total,
                        material_price_total_src_in = item.material_price_total_src,
                        material_attribute_in = item.material_attribute
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
        /// Nomenclature Material Del
        /// </summary>
        public static async Task NomenclatureMaterialDel(string nomenclatureMaterialGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_nomenclature_material_del", new
                    {
                        nomenclature_material_guid_in = nomenclatureMaterialGuid,
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
        /// Nomenclature Material Del
        /// </summary>
        public static async Task NomenclatureMaterialByNomenclatureDel(string nomenclatureGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_nomenclature_material_by_nomenclature_del", new
                    {
                        nomenclature_guid_in = nomenclatureGuid,
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
        /// Nomenclature Operation Get
        /// </summary>
        public static async Task<IEnumerable<NomenclatureOperationEntity>> NomenclatureOperationGet(
            string nomenclatureOperationGuid, string nomenclatureGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<NomenclatureOperationEntity>(
                    "pm_nomenclature_operation_get", new
                    {
                        nomenclature_operation_guid_in = nomenclatureOperationGuid,
                        nomenclature_guid_in = nomenclatureGuid,
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
        /// Nomenclature Material Upd
        /// </summary>
        public static async Task<NomenclatureOperationEntity> NomenclatureOperationUpd(NomenclatureOperationEntity item,
            string userGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<NomenclatureOperationEntity>(
                    "pm_nomenclature_operation_upd", new
                    {
                        nomenclature_operation_guid_in = item.nomenclature_operation_guid,
                        nomenclature_guid_in = item.nomenclature_guid,
                        service_group_guid_in = serviceGroupGuid,
                        user_guid_in = userGuid,
                        operation_guid_in = item.operation_guid,
                        operation_count_in = item.operation_count,
                        operation_price_in = item.operation_price,
                        operation_price_total_in = item.operation_price_total,
                        operation_price_total_src_in = item.operation_price_total_src,
                        operation_attribute_in = item.operation_attribute
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
        /// Nomenclature Operation Del
        /// </summary>
        public static async Task NomenclatureOperationDel(string nomenclatureOperationGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_nomenclature_operation_del", new
                    {
                        nomenclature_operation_guid_in = nomenclatureOperationGuid,
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
        /// Nomenclature Operation Del
        /// </summary>
        public static async Task NomenclatureOperationByNomenclatureDel(string nomenclatureGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_nomenclature_operation_by_nomenclature_del", new
                    {
                        nomenclature_guid_in = nomenclatureGuid,
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
    }
}