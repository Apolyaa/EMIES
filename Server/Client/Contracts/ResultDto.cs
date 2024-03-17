namespace Client.Contracts
{
    public class ResultDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; }
        public List<CharacteristicForFindDto> InitialData { get; set; }
        public List<ResultDeviceDto> ResultsDevices { get; set; }
        public List<DeviceDto> Devices { get; set; }

    }
}
