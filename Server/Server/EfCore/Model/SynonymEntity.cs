namespace Server.EfCore.Model
{
    public class SynonymEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? CharacteristicId { get; set; }
        public DictionaryOfCharacteristicEntity? Characteristic { get; set; }
    }
}
