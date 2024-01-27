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
        public async Task<ActionResult<int>> Create([FromBody] OperationLog operationLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                operationLog.Id = Guid.NewGuid();

                var result = await this._operationLogRepository.AddAsync(operationLog);

                return result == 1
                    ? Ok(new { Message = "Operation log created successfully", OperationLogId = operationLog.Id })
                    : BadRequest(new { Message = $"No records were added. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<int>> Edit([FromBody] OperationLog operationLog)
        {
            if (operationLog == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await this._operationLogRepository.UpdateAsync(operationLog);

                return result == 1
                    ? Ok(new { Message = "Operation log updated successfully", OperationLogId = operationLog.Id })
                    : BadRequest(new { Message = $"No records were updated. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return !await this._operationLogRepository.ExistsAsync(operationLog.Id)
                    ? NotFound()
                    : StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<int>> DeleteConfirmed(Guid id)
        {
            try
            {
                var operationLog = await this._operationLogRepository.GetByIdAsync(id);
                if (operationLog == null)
                {
                    return NotFound();
                }

                var result = await this._operationLogRepository.DeleteAsync(operationLog.Id);

                return result == 1
                    ? Ok(new { Message = "Operation log deleted successfully", OperationLogId = operationLog.Id })
                    : BadRequest(new { Message = $"No records were deleted. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
