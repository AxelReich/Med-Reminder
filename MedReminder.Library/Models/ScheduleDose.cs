using System;

namespace MedReminder.Library.Models
{
    public class ScheduledDose
    {
        public int MedicationId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public decimal QuantityMg { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DoseStatus Status { get; set; }
        public int MinutesOverdue { get; set; }
        public int MinutesUntilDue { get; set; }
    }
    
    public enum DoseStatus
    {
        Overdue,    // Scheduled time passed, no intake record
        DueSoon,    // Due within next hour  
        Upcoming,   // Due later today
        Completed   // Has intake record
    }
}
