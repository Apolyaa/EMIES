namespace Server.EfCore.Model
{
    public class UnitOfMeasurementEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double MultiplicationFactor { get; set; }
    }
}
