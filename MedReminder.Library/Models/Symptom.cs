using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedReminder.Library.Models
{
    public class Symptom
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }
        
        public bool? IsActive { get; set; }

        public List<Stage>? Stages { get; set; }
    }
}
