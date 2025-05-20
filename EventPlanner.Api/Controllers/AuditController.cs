using EventPlanner.Api.Persistence;
using EventPlanner.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Api.Controllers
{
    [Route("api/audit")]
    [ApiController]
    public class AuditController(IAuditService auditService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAuditEntries(
            [FromQuery] string? onderwerp,
            [FromQuery] string? actie)
        {
            var result = await auditService.FilterAsync(onderwerp, actie);
            return Ok(result);
        }
    }
}
