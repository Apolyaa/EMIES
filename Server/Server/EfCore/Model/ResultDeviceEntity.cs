namespace Server.EfCore.Model
{
    public class ResultDeviceEntity
    {
        public Guid Id { get; set; }
        public double PercentEssential { get; set; }
        public double PercentUnessential { get; set; }
        public string[] CharacteristicsResults { get; set; }
        public ResultEntity Result { get; set; }
        public DeviceEntity Device { get; set; }
    }
}
