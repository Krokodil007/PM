using JetBrains.Annotations;
using Newtonsoft.Json;

namespace PM.InfrastructureModule.Common.Data
{
    /// <summary>
    /// Json data Parce
    /// </summary>
    [UsedImplicitly]
    public class JsonDataExtensions
    {
        /// <summary>
        ///  Parce Entity data to json data
        /// </summary>
        public static string EntityToJsonData<T>(T jdata) where T : new()
        {
            var res = jdata != null ? JsonConvert.SerializeObject(jdata) : @"{}";
            return res;
        }

        /// <summary>
        ///  Parce json data to Entity
        /// </summary>
        public static T JsonToEntityData<T>(string jdata) where T : new()
        {
            var item = jdata != null ? JsonConvert.DeserializeObject<T>(jdata) : new T();
            return item;
        }
    }
}