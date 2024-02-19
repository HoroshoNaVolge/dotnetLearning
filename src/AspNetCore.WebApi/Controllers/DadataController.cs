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
        public async Task<ActionResult<string>> GetOrganizationInfo(string inn, CancellationToken token)
        {
            var organizationInfo = await _dadataService.GetOrganizationNameByInnAsync(inn,token);

            if (organizationInfo.IsSuccess) { return Ok(organizationInfo.OrganizationName); }

            else return NotFound(organizationInfo.ErrorDescription);
        }
    }
}
