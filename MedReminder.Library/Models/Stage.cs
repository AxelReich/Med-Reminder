using System;
using MedReminder.Library.Models;

namespace MedReminder.Library.Models;

public class Stage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;  // e.g. "Early", "Mid", "Late"... Only have these three options the last one is to consult a Dr.

    public int SymptomId { get; set; }
    public Symptom Symptom { get; set; } = null!;

    public List<Medication> Medication { get; set; } = new();
}