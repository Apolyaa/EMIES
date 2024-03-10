namespace Client.Contracts
{
    public class TypeOfDeviceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<DictionaryOfCharacteristicDto> MainCharacteristics { get; set; } = new();
    }
}
