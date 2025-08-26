using AdvSwProject.Models;
using System;
using System.Collections.Generic;

namespace AdvSwProject.ViewModels
{
    public class MedicationVm
    {
        public int Id { get; set; }
        public string MedicineName { get; set; } = "";
        public string Dosage { get; set; } = "";
        public string BeforeAfterEating { get; set; } = "";
        public List<TimeSpan> MedicationTimes { get; set; } = new();
    }

    public class AppointmentVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime Date { get; set; }   // التاريخ
        public TimeSpan Time { get; set; }   // الوقت
    }
   

    public class HomeViewModel
    {
        public List<MedicationVm> Medications { get; set; } = new();
        public List<AppointmentVm> Appointments { get; set; } = new();
        public List<HealthTip> HealthTips { get; set; }
    }
}
