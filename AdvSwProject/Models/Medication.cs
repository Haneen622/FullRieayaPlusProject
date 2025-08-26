namespace AdvSwProject.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public string MedicineName { get; set; } = "";
        public string Dosage { get; set; } = "";
        public string BeforeAfterEating { get; set; } = "";
        // نخزن الأوقات كسطر نصي مفصول بفواصل (أسهل مبدئياً)
        public string TimesCsv { get; set; } = ""; // مثال: "19:36,12:36"
    }
}
