using System;
using System.IO;

namespace PM.InfrastructureModule.Entity.Shared
{
    public class AttachmentEntity
    {
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public string attachment_guid { get; set; }
        public string service_group_guid { get; set; }
        public string user_guid { get; set; }

        /// <summary>
        /// Папка
        /// </summary>
        public string container { get; set; }

        /// <summary>
        /// Guid объекта
        /// </summary>
        public string object_guid { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Содержание файла
        /// </summary>
        public MemoryStream content { get; set; }

        /// <summary>
        /// Полный путь к файлу
        /// </summary>
        public string uri { get; set; }

        public bool deleted { get; set; }

        public DateTime dtcreate { get; set; }

        public DateTime dtmodified { get; set; }
    }
}