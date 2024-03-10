namespace Client.Contracts
{
    public class DictionaryOfCharacteristicDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SynonymDto> Synonyms { get; set; } = new();
    }
}
