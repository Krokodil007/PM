namespace PM.InfrastructureModule.Dto.Contact
{
    public class ContactPersonDto
    {
        /// <summary>
        /// Идентификатор контакта
        /// </summary>
        public string contact_person_guid { get; set; }

        /// <summary>
        /// Идентификатор контрагента
        /// </summary>
        public string contact_guid { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string full_name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string sur_name { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string first_name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string middle_name { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string position { get; set; }

        /// <summary>
        /// Телефон 1
        /// </summary>
        public string phone1 { get; set; }

        /// <summary>
        /// Доб. телефон 1
        /// </summary>
        public string phone_add1 { get; set; }

        /// <summary>
        /// Телефон 2
        /// </summary>
        public string phone2 { get; set; }

        /// <summary>
        /// Доб. телефон 2
        /// </summary>
        public string phone_add2 { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public string comment { get; set; }
    }
}