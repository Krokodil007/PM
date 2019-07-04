using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using JetBrains.Annotations;
using PM.InfrastructureModule.Common.Mail;
using PM.InfrastructureModule.DataCore;
using PM.InfrastructureModule.Entity.Catalog;

namespace PM.InfrastructureModule.Repository.StaticQuery.Catalog
{
    /// <summary>
    /// CatalogObject
    /// </summary>
    [UsedImplicitly]
    public class UniversalCatalogInfo
    {
        /// <summary>
        /// CatalogObjectGet
        /// </summary>
        public static async Task<IEnumerable<UniversalCatalogEntity>> CatalogObjectGet(string objectTypeName, string objectGuid,
            string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<UniversalCatalogEntity>(
                    "catalog_object_by_type_name_get",
                    new
                    {
                        object_type_name_in = objectTypeName,
                        object_guid_in = objectGuid,
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
        /// TypeOfOwnership Upd
        /// </summary>
        public static async Task<IEnumerable<UniversalCatalogEntity>> CatalogObjectUpd(string objectGuid, string objectTypeGuid,
            string serviceGroupGuid, string userGuid, string objectInfo)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<UniversalCatalogEntity>(
                    "catalog_object_upd", new
                    {
                        object_guid_in = objectGuid,
                        object_type_guid_in = objectTypeGuid,
                        service_group_guid_in = serviceGroupGuid,
                        user_guid_in = userGuid,
                        object_info_in = objectInfo
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
        /// CatalogObjectDel
        /// </summary>
        public static async Task CatalogObjectDel(string objectGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("catalog_object_del", new
                    {
                        object_guid_in = objectGuid,
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