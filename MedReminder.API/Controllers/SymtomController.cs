
using Microsoft.AspNetCore.Mvc;
using MedReminder.Library.Models;
using MedReminder.API.Enterprise;
using System.Collections.Generic;

namespace MedReminder.API.Controllers
{
    [ApiController]
    [Route("api/symptoms")]
    public class SymptomController : ControllerBase
    {
        private readonly SymptomEC _symptomService;

        public SymptomController(SymptomEC symptomService)
        {
            _symptomService = symptomService;
        }

        [HttpGet]
        public ActionResult<List<Symptom>> GetAll()
        {
            var symptoms = _symptomService.GetAllSymptoms();
            return Ok(symptoms);
        }

        [HttpGet("{id}")]
        public ActionResult<Symptom> GetById(int id)
        {
            var symptom = _symptomService.GetSymptomById(id);
            if (symptom == null) return NotFound();
            return Ok(symptom);
        }

        [HttpPost]
        public ActionResult<Symptom> Create(Symptom symptom)
        {
            try
            {
                var created = _symptomService.AddOrUpdateSymptom(symptom);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Symptom symptom)
        {
            if (id != symptom.Id) return BadRequest("ID mismatch");

            try
            {
                var updated = _symptomService.AddOrUpdateSymptom(symptom);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _symptomService.DeleteSymptom(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
