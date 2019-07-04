namespace PM.InfrastructureModule.Entity.Identity
{
    /// <summary>
    /// ServiceGroup Legal Entity
    /// </summary>
    public class ServiceGroupLegalEntity
    {
        /// <summary>
        /// Идентификатор юр лица
        /// </summary>
        public long LegalEntityId { get; set; }

        /// <summary>
        /// Идентификатор группы компаний
        /// </summary>
        public int ServiceGroupId { get; set; }

        /// <summary>
        /// Форма юридического лица
        /// </summary>
        public int TypeOfOwnershipId { get; set; }

        /// <summary>
        /// Форма юридического лица
        /// </summary>
        public string TypeOfOwnershipName { get; set; }

        /// <summary>
        /// Полное наименование (без формы юридического лица)
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public int Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public int Kpp { get; set; }

        /// <summary>
        /// Наименование банка
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Расчетный счет
        /// </summary>
        public int CheckingAccount { get; set; }

        /// <summary>
        /// Корреспондентский счет
        /// </summary>
        public int CorrespondentAccount { get; set; }

        /// <summary>
        /// БИК
        /// </summary>
        public int Bik { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public string Ogrn { get; set; }

        /// <summary>
        /// ОКПО
        /// </summary>
        public string Okpo { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public string Okato { get; set; }

        /// <summary>
        /// ОКВЭД
        /// </summary>
        public string Okved { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string LegalAddress { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Контактный телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Добавочный телефон
        /// </summary>
        public string PhoneAdd { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Сайт
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Генеральный директор (в именительном падеже)
        /// </summary>
        public string DirectorGeneralNominative { get; set; }

        /// <summary>
        /// Генеральный директор (в родительном падеже)
        /// </summary>
        public string DirectorGeneralGenitive { get; set; }

        /// <summary>
        /// Главный бухгалтер (в именительном падеже)
        /// </summary>
        public string ChiefAccountantNominative { get; set; }

        /// <summary>
        /// Главный бухгалтер (в родительном падеже)
        /// </summary>
        public string ChiefAccountantGenitive { get; set; }
    }
}