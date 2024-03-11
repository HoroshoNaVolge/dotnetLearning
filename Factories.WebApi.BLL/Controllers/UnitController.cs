using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Factories.WebApi.BLL.Controllers
{
    [ApiController]
    [Route("api/unit")]
    public class UnitController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyCollection<Unit>>> GetUnits(CancellationToken token)
        {
            if (unitOfWork == null) { return StatusCode(StatusCodes.Status500InternalServerError); }

            var units = await (unitOfWork.Units?.GetAllAsync(token) ?? throw new NullReferenceException("UoW почему-то ноль или репо ноль"));

            return Ok(units);
        }

        [HttpGet("{id}")]
        public IActionResult GetUnit(int id)
        {
            if (unitOfWork == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var unit = unitOfWork?.Units?.Get(id);

            if (unit == null)
                return NotFound();

            return Ok(unit);
        }

        [HttpPost]
        public IActionResult CreateUnit(Unit unit)
        {
            if (unitOfWork == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            unitOfWork?.Units?.Create(unit);
            unitOfWork?.Save();

            return Ok(unit);
        }

        [HttpPut]
        public IActionResult UpdateUnit(Unit unit)
        {
            if (unitOfWork == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            unitOfWork?.Units?.Update(unit);
            unitOfWork?.Save();

            return Ok(unit);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUnit(int id)
        {
            if (unitOfWork == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            unitOfWork?.Units?.Delete(id);
            unitOfWork?.Save();

            return Ok();
        }
    }
}