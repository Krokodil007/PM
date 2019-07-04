using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.InfrastructureModule.Domain.Services.Catalog
{
    /// <summary>
    /// Справочники
    /// </summary>
    public interface ICatalogDomain
    {
        /// <summary>
        /// Catalog Get
        /// </summary>
        Task<IEnumerable<object>> CatalogGet(string objectTypeName, string objectGuid, string serviceGroupGuid, string filterString);

        /// <summary>
        /// Catalog Upd
        /// </summary>
        Task<object> CatalogUpd(string objectTypeName, string item, string userGuid, string serviceGroupGuid);
    }
}