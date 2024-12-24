namespace WilliamsRacingTechnicalAssessment.Models
{
    /// <summary>
    /// Naming violation to match the JSON name formatting.
    /// </summary>
    public class Circuit
    {
        public int circuitId { get; set; }
        public string circuitRef { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string country { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int alt { get; set; }
        public string url { get; set; }
    }
}
