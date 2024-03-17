namespace Client.Contracts
{
    public class CharacteristicDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public bool IsEssential { get; set; }
        public string Type { get; set; }
        public bool IsMatch { get; set; }
    }
}
