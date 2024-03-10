namespace Client.Contracts
{
    public class ResultDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; }
        public string InitialData { get; set; }

    }
}
