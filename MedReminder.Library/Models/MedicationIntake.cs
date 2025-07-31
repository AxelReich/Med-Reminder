using System;
using System.ComponentModel.DataAnnotations;


namespace MedReminder.Library.Models
{
    public class MedicationIntake
    {
        public int? Id { get; set; }
        public int MedicationId { get; set; }                   // Links to Medication
        public DateTime ScheduledTime { get; set; }             // When they SHOULD have taken it
        public DateTime ActualTime { get; set; }                // When they ACTUALLY took it
                
        // Navigation Properties
        public Medication? Medication { get; set; }
    }
}