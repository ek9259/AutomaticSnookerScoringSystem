namespace SnookerScoringSystem.Domain
{
    // All the code in this file is included in all platforms.
    public class DetectedBall
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}