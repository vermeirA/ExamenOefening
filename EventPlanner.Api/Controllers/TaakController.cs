using System.Threading.Tasks;
using EventPlanner.Api.Contracts.Taak;
using EventPlanner.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Api.Controllers
{
    [Route("api/taken")]
    [ApiController]
    public class TaakController(ITaakService taakService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await taakService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var taak = await taakService.GetByIdAsync(id);
            if (taak == null)
            {
                return NotFound();
            }
            return Ok(taak);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaakRequestContract request)
        {
            var createdTaak = await taakService.AddAsync(request);
            if (createdTaak == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new { id = createdTaak.Id }, createdTaak);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TaakRequestContract request)
        {
            var updatedTaak = await taakService.UpdateAsync(id, request);
            if (updatedTaak == null)
            {
                return NotFound();
            }
            return Ok(updatedTaak);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await taakService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PatchStatus([FromRoute] int id, [FromBody] TaakPatchStatusRequestContract request)
        {
            await taakService.PatchStatusAsync(id, request);
            return Ok();
        }
    }
}
