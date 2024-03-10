namespace Client.Contracts
{
    public class UnitOfMesurementDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double MultiplicationFactor { get; set; }
    }
}
