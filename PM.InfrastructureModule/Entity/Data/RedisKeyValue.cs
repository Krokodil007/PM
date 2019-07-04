using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace PM.InfrastructureModule.Entity.Data
{
    /// <summary>
    /// KeyValue
    /// </summary>
    [UsedImplicitly]
    public class RedisKeyValue
    {
        [Required]
        public string key { get; set; }

        public string value { get; set; }

        public string key_expire { get; set; }
    }
}