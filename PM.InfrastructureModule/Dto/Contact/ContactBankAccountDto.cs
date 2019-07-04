namespace PM.InfrastructureModule.Dto.Contact
{
    public class ContactBankAccountDto
    {
        public string contact_bank_account_guid { get; set; }

        public string contact_guid { get; set; }

        public int? inn { get; set; }

        public int? account_number { get; set; }

        public int? kpp { get; set; }

        public int? checking_account { get; set; }

        public string bank_name { get; set; }

        public int? correspondent_account { get; set; }

        public int? okpo { get; set; }

        public int? okved1 { get; set; }

        public int? okved2 { get; set; }

        public int? okved3 { get; set; }

        public int? okved4 { get; set; }

        public int? okved5 { get; set; }

        public bool is_main { get; set; }

        public decimal? vat { get; set; }

        public string bik { get; set; }

        public string ogrn { get; set; }
    }
}