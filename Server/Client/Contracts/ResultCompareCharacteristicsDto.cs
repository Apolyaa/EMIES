namespace Client.Contracts
{
    public class ResultCompareCharacteristicsDto
    {
        public int CountNotFoundCharacteristics { get; set; }
        public int CountSuitableEssential { get; set; }
        public int CountSuitableUnessential { get; set; }
        public List<CharacteristicDto> Characteristics { get; set; } = new();
    }
}
