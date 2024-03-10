namespace Server.EfCore.Model
{
    public class DeviceEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Url { get; set; }
        public Guid ProducerId { get; set; }
        public Guid SourceId { get; set; }
        public Guid TypeId { get; set; }
        public ProducerEntity Producer { get; set; }
        public SourceEntity Source { get; set; }
        public TypeOfDevicesEntity Type { get; set; }
    }
}
