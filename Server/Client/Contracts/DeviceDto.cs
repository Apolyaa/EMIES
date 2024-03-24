namespace Client.Contracts
{
    public class DeviceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public ProducerDto Producer { get; set; }
        public SourceDto Source { get; set; }
        public List<CharacteristicDto> Characteristics { get; set; }
        public TypeOfDeviceDto Type { get; set; }
    }
}
