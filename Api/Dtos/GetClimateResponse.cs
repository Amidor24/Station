namespace Api.Dtos
{
    public class GetClimateResponse
    {
        public DateTime Date { get; set; }
        public double Intensity { get; set; }
        public double DailyRain { get; set; }
        public double TotalRain { get; set; }
    }
}
