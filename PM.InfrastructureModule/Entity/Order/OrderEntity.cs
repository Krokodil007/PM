using System;

namespace PM.InfrastructureModule.Entity.Order
{
    public class OrderEntity
    {
        public string order_guid { get; set; }

        public string contact_guid { get; set; }

        public string order_type_guid { get; set; }

        public string order_status_guid { get; set; }

        public string user_fullname { get; set; }

        public string contact_name { get; set; }

        public string order_type_name { get; set; }

        public string order_status_name { get; set; }

        public string order_info { get; set; }

        public bool deleted { get; set; }

        public DateTime dtcreate { get; set; }

        public DateTime dtmodified { get; set; }
    }
}