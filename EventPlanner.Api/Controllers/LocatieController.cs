using EventPlanner.Api.Contracts.Evenement;
using EventPlanner.Api.Contracts.Locatie;
using EventPlanner.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Api.Controllers
{
    [Route("api/locaties")]
    [ApiController]
    public class LocatieController(ILocatieService locatieService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await locatieService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await locatieService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocatieRequestContract locatie)
        {
            var createdLocation = await locatieService.AddAsync(locatie);
            return CreatedAtAction(nameof(GetById), new { id = createdLocation.Id }, createdLocation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LocatieRequestContract locatie)
        {
            var updated = await locatieService.UpdateAsync(id, locatie);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await locatieService.DeleteAsync(id);
            return NoContent();
        }
    }
}
