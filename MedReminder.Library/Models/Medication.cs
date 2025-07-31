using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace MedReminder.Library.Models
{
    public class Medication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int IntervalHours { get; set; }

        [Range(1, int.MaxValue)]
        public int TotalDays { get; set; }

        [Range(1, 10000)]
        public int QuantityMg { get; set; }

        public int TreatmentId { get; set; }

        public Treatment Treatment { get; set; } = null!;

        public List<MedicationDose> Doses { get; set; } = new();
    }
}



