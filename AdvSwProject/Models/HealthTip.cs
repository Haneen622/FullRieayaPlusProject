namespace AdvSwProject.Models
{
    public class HealthTip
    {
        public int Id { get; set; }
        public string Title { get; set; }   // Stay Hydrated, Daily Walk
        public string Description { get; set; } // النص التفصيلي
        public string? VideoUrl { get; set; }   // لينك للفيديو (اختياري)
    }

}
