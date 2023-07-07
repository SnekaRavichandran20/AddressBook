namespace AddressBookApi.Dtos
{
    public record RefSetDto
    {
        public Guid id { get; set; }
        public string? Key { get; set; }
        public string? display { get; set; }
        public string? description { get; set; }
        public DiplaysDto? displays { get; set; }
    }

    public record DiplaysDto
    {
        public string? en { get; set; }
    }

}


