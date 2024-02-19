using AspNetCore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DadataController : ControllerBase
    {
        private readonly IDadataService _dadataService;

        public DadataController(IDadataService dadataService)
        {
            _dadataService = dadataService;
        }

        [HttpGet(template: "get")]
        public async Task<ActionResult<string>> GetOrganizationInfo(string inn)
        {
            var organizationName = await _dadataService.GetOrganizationNameByInnAsync(inn);

            if (organizationName != null) { return Ok(organizationName); }

            else return NotFound($"Не найдена компания с ИНН: {inn}");
        }
    }
}
