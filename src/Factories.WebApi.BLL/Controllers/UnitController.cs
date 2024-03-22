using AutoMapper;
using Factories.WebApi.BLL.Dto;
using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Factories.WebApi.BLL.Controllers
{
    [ApiController]
    [Route("api/unit")]
    public class UnitController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        private readonly IMapper mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyCollection<UnitDto>>> GetUnits(CancellationToken token)
        {
            var units = await (unitOfWork.Units.GetAllAsync(token) ?? throw new NullReferenceException("UoW почему-то ноль или репо ноль"));

            return Ok(mapper.Map<IReadOnlyCollection<UnitDto>>(units));
        }

        [HttpGet("{id}")]
        public IActionResult GetUnit(int id)
        {
            var unit = unitOfWork.Units?.Get(id);

            if (unit == null)
                return NotFound();

            return Ok(mapper.Map<UnitDto>(unit));
        }

        [HttpPost]
        public IActionResult CreateUnit(Unit unit)
        {
            unitOfWork.Units?.Create(unit);
            unitOfWork.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateUnit(int id, Unit unit)
        {
            unitOfWork.Units?.Update(id, unit);
            unitOfWork.Save();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUnit(int id)
        {
            unitOfWork.Units?.Delete(id);
            unitOfWork.Save();

            return Ok();
        }
    }
}