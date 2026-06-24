namespace BackBlaze.Client.IService.Models
{
    public class LegalHold
    {
        public bool isClientAuthorizedToRead { get; set; }
        public string? value { get; set; }
    }
}
