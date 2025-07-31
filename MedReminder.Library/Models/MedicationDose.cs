using System;
using System.ComponentModel.DataAnnotations;


namespace MedReminder.Library.Models;

public class MedicationDose
{
    [Key]
    public int Id { get; set; }

    // When the dose is scheduled
    public DateTime ScheduledTime { get; set; }

    // Whether the user has taken this dose
    public bool IsTaken { get; set; }

    public int MedicationId { get; set; }

    public Medication Medication { get; set; } = null!;
}
