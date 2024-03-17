namespace Server.EfCore.Model
{
    public class CharacteristicEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double NumberValue { get; set; }
        public string StringValue { get; set; }
        public string[] ArrayOfValues { get; set; }
        public bool BooleanValue { get; set; }
        public string Type { get; set; }
        public Guid UnitId { get; set; }
        public Guid DeviceId { get; set; }
        public UnitOfMeasurementEntity UnitOfMeasurement { get; set; }
        public DeviceEntity Device { get; set; }

    }
}
