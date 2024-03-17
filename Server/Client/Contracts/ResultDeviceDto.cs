namespace Client.Contracts
{
    public class ResultDeviceDto
    {
        public Guid Id { get; set; }
        public Guid ResultId { get; set; }
        public Guid DeviceId { get; set; }
        public double PercentEssential { get; set; }
        public double PercentUnessential { get; set; }
        public List<CharacteristicDto> CharacteristicsResults { get; set; }
    }
}
