using System;
using MedReminder.Library.Models;

namespace MedReminder.Library.Models;

public class Stage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;  // e.g. "Early", "Mid", "Late"

    public int SymptomId { get; set; }
    public Symptom Symptom { get; set; } = null!;

    public List<Treatment> Treatments { get; set; } = new();
}