namespace WilliamsRacingTechnicalAssessment.Models
{
    /// <summary>
    /// Naming violation to match the JSON name formatting.
    /// </summary>
    public class Driver
    {
        public int driverId { get; set; }
        public string driverRef { get; set; }
        public string number { get; set; }
        public string code { get; set; }
        public string forename { get; set; }
        public string surname { get; set; }
        public string dob { get; set; }
        public string nationality { get; set; }
        public string url { get; set; }
    }
}
