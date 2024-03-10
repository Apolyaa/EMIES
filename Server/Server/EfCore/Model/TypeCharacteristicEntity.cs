namespace Server.EfCore.Model
{
    public class TypeCharacteristicEntity
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public Guid CharacteristicId { get; set; }
        public DictionaryOfCharacteristicEntity Characteristic { get; set; }
        public TypeOfDevicesEntity Type { get; set; }
    }
}
