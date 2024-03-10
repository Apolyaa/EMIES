namespace Server.EfCore.Model
{
    public class ProducerEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Contacts { get; set; }
        public string? Description { get; set; }
    }
}
