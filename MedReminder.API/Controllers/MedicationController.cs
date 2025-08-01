using Microsoft.AspNetCore.Mvc;
using MedReminder.API.Enterprise;
using MedReminder.Library.Models;

namespace MedReminder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationController : ControllerBase
    {
        private readonly MedicationEC _medicationEC;

        public MedicationController(MedicationEC medicationEC)
        {
            _medicationEC = medicationEC;
        }

        [HttpGet("stage/{stageId}")]
        public IActionResult GetMedicationsByStage(int stageId)
        {
            var meds = _medicationEC.GetMedicationsByStage(stageId);
            return Ok(meds);
        }

        [HttpGet("{id}")]
        public IActionResult GetMedication(int id)
        {
            var med = _medicationEC.GetMedication(id);
            if (med == null)
                return NotFound();

            return Ok(med);
        }

        // POST api/medication
        [HttpPost]
        public IActionResult AddMedication([FromBody] Medication medication)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newId = _medicationEC.AddMedication(medication);
            medication.Id = newId;

            return CreatedAtAction(nameof(GetMedication), new { id = newId }, medication);
        }

        // PUT api/medication/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateMedication(int id, [FromBody] Medication medication)
        {
            if (id != medication.Id)
                return BadRequest("ID mismatch");

            if (!_medicationEC.UpdateMedication(medication))
                return NotFound();

            return NoContent();
        }

        // DELETE api/medication/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteMedication(int id)
        {
            if (!_medicationEC.DeleteMedication(id))
                return NotFound();

            return NoContent();
        }
    }
}
