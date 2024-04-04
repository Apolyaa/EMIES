namespace Client.Contracts
{
    public class UnitOfMeasurementDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double MultiplicationFactor { get; set; }
    }
}
