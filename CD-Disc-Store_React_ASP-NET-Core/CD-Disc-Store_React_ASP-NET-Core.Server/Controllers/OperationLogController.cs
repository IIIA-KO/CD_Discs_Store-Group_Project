using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperationLogController : Controller
    {
        private readonly IOperationLogRepository _operationLogRepository;

        public OperationLogController(IOperationLogRepository operationLogRepository)
        {
            this._operationLogRepository = operationLogRepository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<OperationLog>>> GetAll()
        {
            return Ok(await this._operationLogRepository.GetAllAsync());
        }

        [HttpGet("GetOperationLog")]
        public async Task<ActionResult<OperationLog>> GetOperationLog(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var operationLog = await this._operationLogRepository.GetByIdAsync(id);
                return Ok(operationLog);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("Client/{id}")]
        public async Task<ActionResult<OperationLog>> GetByClientOperationLog(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationLog = await this._operationLogRepository.GetByClientIdAsync(id);

            if (operationLog == null)
            {
                return NotFound();
            }

            return Ok(operationLog);
        }

        [HttpGet("Disc/{id}")]
        public async Task<ActionResult<OperationLog>> GetByDiscOperationLog(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationLog = await this._operationLogRepository.GetByDiscIdAsync(id);

            if (operationLog == null)
            {
                return NotFound();
            }

            return Ok(operationLog);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<int>> Create([Bind("Id,OperationDateTimeStart,OperationDateTimeEnd,ClientId,DiscId,OperationType,Quantity")] OperationLog operationLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                operationLog.Id = Guid.NewGuid();

                return await this._operationLogRepository.AddAsync(operationLog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<int>> Edit(Guid? id, [Bind("Id,OperationDateTimeStart,OperationDateTimeEnd,ClientId,DiscId,OperationType,Quantity")] OperationLog operationLog)
        {
            if (id == null || operationLog == null)
            {
                return BadRequest(ModelState);
            }

            if (id != operationLog.Id)
            {
                return BadRequest();
            }

            try
            {
                return Ok(await this._operationLogRepository.UpdateAsync(operationLog));
            }
            catch (Exception ex)
            {
                if (!await this._operationLogRepository.ExistsAsync(operationLog.Id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<int>> DeleteConfirmed(Guid id)
        {
            var operationLog = await this._operationLogRepository.GetByIdAsync(id);

            return operationLog == null ? NotFound()
                : Ok(await this._operationLogRepository.DeleteAsync(operationLog.Id));
        }
    }
}