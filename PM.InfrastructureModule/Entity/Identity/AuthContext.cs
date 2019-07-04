using System.Collections.Generic;

namespace PM.InfrastructureModule.Entity.Identity
{
    public class AuthContext
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Guid { get; set; }

        public string service_group_guid { get; set; }

        public List<SimpleClaim> Claims { get; set; }

        public List<string> Roles { get; set; }
    }
}