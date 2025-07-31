using System;
using System.Data.Common;
using System.Collections.Generic;
using MedReminder.Library.Models;
using System.Dynamic;



namespace MedReminder.Library.Models
{
    public class Treatment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Toggle to determine if reminders should be sent
        public bool IsActive { get; set; }
        
        public int StageId { get; set; }
        public Stage Stage { get; set; } = null!;       // Early, Mid, Severe always severe is consult a doctor 

        public List<Medication> Medications { get; set; } = new();
    }
}

// id,  Name of sympton, stage of the sympton . Call dctr 