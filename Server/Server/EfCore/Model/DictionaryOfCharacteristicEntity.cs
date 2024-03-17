namespace Server.EfCore.Model
{
    public class DictionaryOfCharacteristicEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string[] Synonyms { get; set; }
    }
}
