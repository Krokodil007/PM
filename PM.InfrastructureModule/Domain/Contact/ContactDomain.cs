using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PM.InfrastructureModule.Common.Data;
using PM.InfrastructureModule.Domain.Services.Contact;
using PM.InfrastructureModule.Dto.Contact;
using PM.InfrastructureModule.Entity.Contact;
using PM.InfrastructureModule.Repository.StaticQuery.Contact;

namespace PM.InfrastructureModule.Domain.Contact
{
    /// <summary>
    /// Контрагенты
    /// </summary>
    public class ContactDomain : IContactDomain
    {
        /// <summary>
        /// Contact Get
        /// </summary>
        public async Task<IEnumerable<ContactDto>> ContactGet(string contactGuid, string serviceGroupGuid)
        {
            var contacts = await ContactInfo.ContactGet(contactGuid, serviceGroupGuid);
            var result = contacts.Select(c => JsonDataExtensions.JsonToEntityData<ContactDto>(c.contact_info)).ToList();
            if (string.IsNullOrEmpty(contactGuid) || !result.Any()) return result;

            var account = await ContactBankAccountGet(null, contactGuid, serviceGroupGuid);
            result[0].contact_bank_account_guid = account.Select(a => a.contact_bank_account_guid).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Contact Upd
        /// </summary>
        public async Task<ContactDto> ContactUpd(ContactDto dtoItem, string userGuid, string serviceGroupGuid)
        {
            var item = new ContactEntity
            {
                contact_guid = dtoItem.contact_guid,
                contact_type_guid = dtoItem.contact_type_guid,
                contact_info = JsonDataExtensions.EntityToJsonData(dtoItem)
            };

            var contact = await ContactInfo.ContactUpd(item, userGuid, serviceGroupGuid);
            var result = JsonDataExtensions.JsonToEntityData<ContactDto>(contact.contact_info);
            return result;
        }

        /// <summary>
        /// Contact Bank Account Get
        /// </summary>
        public async Task<IEnumerable<ContactBankAccountDto>> ContactBankAccountGet(string contactBankAccountGuid,
            string contactGuid, string serviceGroupGuid)
        {
            var accounts = await ContactInfo.ContactBankAccountGet(contactBankAccountGuid, contactGuid, serviceGroupGuid);
            var result = accounts.Select(c =>
                JsonDataExtensions.JsonToEntityData<ContactBankAccountDto>(c.contact_bank_account_info));
            return result;
        }

        /// <summary>
        /// Contact Bank Account Upd
        /// </summary>
        public async Task<IEnumerable<ContactBankAccountDto>> ContactBankAccountUpd(List<ContactBankAccountDto> dtoItems, string userGuid, string serviceGroupGuid)
        {
            foreach (var dtoItem in dtoItems)
            {
                var item = new ContactBankAccountEntity
                {
                    contact_bank_account_guid = dtoItem.contact_bank_account_guid,
                    contact_guid = dtoItem.contact_guid,
                    contact_bank_account_info = JsonDataExtensions.EntityToJsonData(dtoItem)
                };
                await ContactInfo.ContactBankAccountUpd(item, userGuid, serviceGroupGuid);
            }

            var result =
                await ContactBankAccountGet(null, dtoItems.Select(c => c.contact_guid).FirstOrDefault(), serviceGroupGuid);
            return result;
        }

        /// <summary>
        /// Contact Person Get
        /// </summary>
        public async Task<IEnumerable<ContactPersonDto>> ContactPersonGet(string contactPersonGuid, string contactGuid, string serviceGroupGuid)
        {
            var persons = await ContactInfo.ContactPersonGet(contactPersonGuid, contactGuid, serviceGroupGuid);
            var result = persons.Select(c =>
                JsonDataExtensions.JsonToEntityData<ContactPersonDto>(c.contact_person_info));
            return result;
        }

        /// <summary>
        /// Contact Person Upd
        /// </summary>
        public async Task<IEnumerable<ContactPersonDto>> ContactPersonUpd(List<ContactPersonDto> dtoItems, string userGuid, string serviceGroupGuid)
        {
            foreach (var dtoItem in dtoItems)
            {
                var item = new ContactPersonEntity()
                {
                    contact_person_guid = dtoItem.contact_person_guid,
                    contact_guid = dtoItem.contact_guid,
                    contact_person_info = JsonDataExtensions.EntityToJsonData(dtoItem)
                };
                await ContactInfo.ContactPersonUpd(item, userGuid, serviceGroupGuid);
            }

            var result = await ContactPersonGet(null, dtoItems.Select(c => c.contact_guid).FirstOrDefault(), serviceGroupGuid);
            return result;
        }
    }
}