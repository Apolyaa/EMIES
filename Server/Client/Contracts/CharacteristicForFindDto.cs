namespace Client.Contracts
{
    public class CharacteristicForFindDto
    {
        public string Name { get; set; }    
        public string Value { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string TypeCharacteristic { get; set; }
        public bool IsEssential { get; set; }
    }
}
