using AutoMapper;
using Factories.WebApi.BLL.Dto;
using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Factories.WebApi.BLL.Controllers
{
    [ApiController]
    [Route("api/tank")]
    public class TankController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        private readonly IMapper mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        [HttpGet(template: "all")]
        public async Task<ActionResult<IReadOnlyCollection<TankDto>>> GetTanks(CancellationToken token)
        {
            var tanks = await (unitOfWork.Tanks.GetAllAsync(token) ?? throw new NullReferenceException("UoW почему-то ноль или репо ноль"));

            var tankDtos = mapper.Map<IReadOnlyCollection<TankDto>>(tanks);

            return Ok(tankDtos);
        }

        [HttpGet(template: "{id}")]
        public IActionResult GetTank(int id)
        {
            var tank = unitOfWork.Tanks.Get(id);

            if (tank == null) { return NotFound(); }

            var tankDto = mapper.Map<TankDto>(tank);

            return Ok(tankDto);
        }

        [HttpPost]
        public IActionResult CreateTank(Tank tank)
        {
            unitOfWork.Tanks.Create(tank);

            unitOfWork.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateTank(int id, Tank tank)
        {
            unitOfWork.Tanks.Update(id, tank);

            unitOfWork.Save();

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteTank(int id)
        {
            unitOfWork.Tanks.Delete(id);

            unitOfWork.Save();

            return Ok();
        }
    }
}