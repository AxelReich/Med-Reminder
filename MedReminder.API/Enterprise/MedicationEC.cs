using System.Collections.Generic;
using MedReminder.API.Database;
using MedReminder.Library.Models;

namespace MedReminder.API.Enterprise
{
    public class MedicationEC
    {
        private readonly MedicationRepoSqliteContext _medicationRepo;

        public MedicationEC(MedicationRepoSqliteContext medicationRepo)
        {
            _medicationRepo = medicationRepo;
        }

        public List<Medication> GetMedicationsByStage(int stageId)
        {
            return _medicationRepo.GetByStageId(stageId);
        }

        public Medication? GetMedication(int id)
        {
            return _medicationRepo.GetById(id);
        }

        public int AddMedication(Medication medication)
        {
            return _medicationRepo.Add(medication);
        }

        public bool UpdateMedication(Medication medication)
        {
            return _medicationRepo.Update(medication);
        }

        public bool DeleteMedication(int id)
        {
            return _medicationRepo.Delete(id);
        }
    }
}

