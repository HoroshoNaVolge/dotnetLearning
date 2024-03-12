using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Factories.WebApi.BLL.Controllers
{
    [ApiController]
    [Route("api/tank")]
    public class TankController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet(template: "all")]
        public async Task<ActionResult<IReadOnlyCollection<Tank>>> GetTanks(CancellationToken token)
        {
            if (unitOfWork == null) { return StatusCode(StatusCodes.Status500InternalServerError); }

            var tanks = await (unitOfWork.Tanks?.GetAllAsync(token) ?? throw new NullReferenceException("UoW почему-то ноль или репо ноль"));

            return Ok(tanks);
        }

        [HttpGet(template: "{id}")]
        public IActionResult GetTank(int id)
        {
            if (unitOfWork == null) { return StatusCode(StatusCodes.Status500InternalServerError); }
            var tank = unitOfWork.Tanks.Get(id);

            if (tank == null) { return NotFound(); }

            return Ok(tank);
        }

        [HttpPost]
        public IActionResult CreateTank(Tank tank)
        {
            if (unitOfWork == null) { return StatusCode(StatusCodes.Status500InternalServerError); }
            unitOfWork.Tanks.Create(tank);
            unitOfWork.Save();

            return Ok(tank);
        }

        [HttpPut]
        public IActionResult UpdateTank(int id, Tank tank)
        {
            if (unitOfWork == null) { return StatusCode(StatusCodes.Status500InternalServerError); }
            unitOfWork.Tanks.Update(id, tank);
            unitOfWork.Save();

            return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteTank(int id)
        {
            if (unitOfWork == null) { return StatusCode(StatusCodes.Status500InternalServerError); }
            unitOfWork.Tanks.Delete(id);
            unitOfWork.Save();

            return Ok();
        }
    }
}
