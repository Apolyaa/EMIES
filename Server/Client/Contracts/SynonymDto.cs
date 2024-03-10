namespace Client.Contracts
{
    public class SynonymDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? CharacteristicId { get; set; }
    }
}
