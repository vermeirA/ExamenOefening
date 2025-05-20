using EventPlanner.Api.Contracts.Evenement;
using EventPlanner.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Api.Controllers
{
    [Route("api/evenementen")]
    [ApiController]
    public class EvenementController(IEvenementService evenementService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await evenementService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var evenement = await evenementService.GetByIdAsync(id);
            if (evenement == null)
                return NotFound();
            return Ok(evenement);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EvenementRequestContract request)
        {
            var createdEvenement = await evenementService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdEvenement.Id }, createdEvenement);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EvenementRequestContract request)
        {
            var updated = await evenementService.UpdateAsync(id, request);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await evenementService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/rapport")]
        public async Task<IActionResult> GetOverzichtRapport(int id)
        {
            var rapport = await evenementService.GetOverzichtRapportAsync(id);
            if (rapport == null)
                return NotFound();

            return Ok(rapport);
        }
    }
}
