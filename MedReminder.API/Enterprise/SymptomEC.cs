using MedReminder.Library.Models;
using MedReminder.API.Database;
using System.Collections.Generic;

namespace MedReminder.API.Enterprise
{
    public class SymptomEC
    {
        private readonly SymptomRepository _symptomRepository;

        public SymptomEC(SymptomRepository symptomRepository)
        {
            _symptomRepository = symptomRepository;
        }

        public List<Symptom> GetAllSymptoms()
        {
            return _symptomRepository.GetAll();
        }

        public Symptom? GetSymptomById(int id)
        {
            return _symptomRepository.GetById(id);
        }

        public Symptom AddOrUpdateSymptom(Symptom symptom)
        {
            // Example business logic: validate name is not empty
            if (string.IsNullOrWhiteSpace(symptom.Name))
            {
                throw new ArgumentException("Symptom name cannot be empty.");
            }

            return _symptomRepository.AddOrUpdate(symptom);
        }

        public bool DeleteSymptom(int id)
        {
            // Example business rule: don't delete symptom if linked to active stages (pseudo code)
            // if (_stageRepository.HasActiveStages(id))
            //    throw new InvalidOperationException("Cannot delete symptom with active stages.");

            return _symptomRepository.Delete(id);
        }
    }
}
