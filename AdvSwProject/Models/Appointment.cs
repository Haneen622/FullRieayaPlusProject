namespace AdvSwProject.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime Date { get; set; }   // التاريخ
        public TimeSpan Time { get; set; }   // الوقت
    }
}
