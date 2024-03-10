namespace Server.EfCore.Model
{
    public class ResultEntity
    {
        public Guid Id { get; set; }
        public string InitialData { get; set; } = string.Empty;
        public UserEntity User { get; set; }
    }
}
