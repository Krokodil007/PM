using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using JetBrains.Annotations;
using PM.InfrastructureModule.Common.Mail;
using PM.InfrastructureModule.DataCore;
using PM.InfrastructureModule.Entity.Contact;

namespace PM.InfrastructureModule.Repository.StaticQuery.Contact
{
    /// <summary>
    /// Log DataStore Info
    /// </summary>
    [UsedImplicitly]
    public class ContactInfo
    {
        /// <summary>
        /// Contact Get
        /// </summary>
        public static async Task<IEnumerable<ContactEntity>> ContactGet(string contactGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<ContactEntity>("pm_contact_get", new
                {
                    contact_guid_in = contactGuid,
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
        /// Contact Upd
        /// </summary>
        public static async Task<ContactEntity> ContactUpd(ContactEntity item, string userGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<ContactEntity>(
                    "pm_contact_upd", new
                    {
                        contact_guid_in = item.contact_guid,
                        service_group_guid_in = serviceGroupGuid,
                        user_guid_in = userGuid,
                        contact_type_guid_in = item.contact_type_guid,
                        contact_info_in = item.contact_info
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
        /// Contact Upd
        /// </summary>
        public static async Task ContactApprovedUpd(string contactGuid, bool approved, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync(
                    "pm_contact_approved_upd", new
                    {
                        contact_guid_in = contactGuid,
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
        /// Contact Del
        /// </summary>
        public static async Task ContactDel(string contactGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_contact_del", new
                {
                    contact_guid_in = contactGuid,
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
        /// Contact Bank Account Get
        /// </summary>
        public static async Task<IEnumerable<ContactBankAccountEntity>> ContactBankAccountGet(string contactBankAccountGuid, string contactGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<ContactBankAccountEntity>("pm_contact_bank_account_get", new
                {
                    contact_bank_account_guid_in = contactBankAccountGuid,
                    contact_guid_in = contactGuid,
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
        /// Contact Bank Account Upd
        /// </summary>
        public static async Task<ContactBankAccountEntity> ContactBankAccountUpd(ContactBankAccountEntity item, string userGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<ContactBankAccountEntity>(
                    "pm_contact_bank_account_upd", new
                    {
                        contact_bank_account_guid_in = item.contact_bank_account_guid,
                        contact_guid_in = item.contact_guid,
                        service_group_guid_in = serviceGroupGuid,
                        user_guid_in = userGuid,
                        contact_bank_account_info_in = item.contact_bank_account_info
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
        /// Contact Bank Account Del
        /// </summary>
        public static async Task ContactBankAccountDel(string contactBankAccountGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_contact_bank_account_del", new
                {
                    contact_bank_account_guid_in = contactBankAccountGuid,
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
        /// Contact Person Get
        /// </summary>
        public static async Task<IEnumerable<ContactPersonEntity>> ContactPersonGet(string contactPersonGuid, string contactGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<ContactPersonEntity>("pm_contact_person_get", new
                {
                    contact_person_guid_in = contactPersonGuid,
                    contact_guid_in = contactGuid,
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
        /// Contact Person Upd
        /// </summary>
        public static async Task<ContactPersonEntity> ContactPersonUpd(ContactPersonEntity item, string userGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<ContactPersonEntity>(
                    "pm_contact_person_upd", new
                    {
                        contact_person_guid_in = item.contact_person_guid,
                        contact_guid_in = item.contact_guid,
                        service_group_guid_in = serviceGroupGuid,
                        user_guid_in = userGuid,
                        contact_person_info_in = item.contact_person_info
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
        /// Contact Person Del
        /// </summary>
        public static async Task ContactPersonDel(string contactPersonGuid, string serviceGroupGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_contact_person_del", new
                {
                    contact_person_guid_in = contactPersonGuid,
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