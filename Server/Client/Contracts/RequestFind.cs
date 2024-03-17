namespace Client.Contracts
{
    public class RequestFind
    {
        public Guid TypeId { get; set; }
        public List<CharacteristicForFindDto> Characteristics { get; set; } = new();
        public UserDto User { get; set; }
    }
}
