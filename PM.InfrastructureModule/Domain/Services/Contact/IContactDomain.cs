using System.Collections.Generic;
using System.Threading.Tasks;
using PM.InfrastructureModule.Dto.Contact;

namespace PM.InfrastructureModule.Domain.Services.Contact
{
    /// <summary>
    /// Контрагенты
    /// </summary>
    public interface IContactDomain
    {
        /// <summary>
        /// Contact Get
        /// </summary>
        Task<IEnumerable<ContactDto>> ContactGet(string contactGuid, string serviceGroupGuid);

        /// <summary>
        /// Contact Upd
        /// </summary>
        Task<ContactDto> ContactUpd(ContactDto dtoItem, string userGuid, string serviceGroupGuid);

        /// <summary>
        /// Contact Bank Account Get
        /// </summary>
        Task<IEnumerable<ContactBankAccountDto>> ContactBankAccountGet(string contactBankAccountGuid,string contactGuid, string serviceGroupGuid);

        /// <summary>
        /// Contact Bank Account Upd
        /// </summary>
        Task<IEnumerable<ContactBankAccountDto>> ContactBankAccountUpd(List<ContactBankAccountDto> dtoItems, string userGuid, string serviceGroupGuid);

        /// <summary>
        /// Contact Person Get
        /// </summary>
        Task<IEnumerable<ContactPersonDto>> ContactPersonGet(string contactPersonGuid, string contactGuid, string serviceGroupGuid);

        /// <summary>
        /// Contact Person Upd
        /// </summary>
        Task<IEnumerable<ContactPersonDto>> ContactPersonUpd(List<ContactPersonDto> dtoItems, string userGuid, string serviceGroupGuid);
    }
}