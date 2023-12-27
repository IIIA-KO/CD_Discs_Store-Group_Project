using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
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

		[HttpGet("Index")]
        public async Task<ActionResult<IReadOnlyList<OperationLog>>> Index()
		{
			return Ok(await this._operationLogRepository.GetAllAsync());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OperationLog>> GetOperationLog(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var operation = await this._operationLogRepository.GetByIdAsync(id);

			if (operation == null)
			{
				return NotFound();
			}

			return Ok(operation);
		}

		[HttpGet("Client/{id}")]
		public async Task<ActionResult<OperationLog>> GetByClientOperationLog(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var operation = await this._operationLogRepository.GetByClientIdAsync(id);

			if (operation == null)
			{
				return NotFound();
			}

			return Ok(operation);
		}

		[HttpGet("Disc/{id}")]
		public async Task<ActionResult<OperationLog>> GetByDiscOperationLog(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var operation = await this._operationLogRepository.GetByDiscIdAsync(id);

			if (operation == null)
			{
				return NotFound();
			}

			return Ok(operation);
		}

		[HttpPost("Create")]
		public async Task<int> Create([Bind("Id,OperationDateTimeStart,OperationDateTimeEnd,ClientId,DiscId,OperationType,Quantity")] OperationLog operationLog)
		{
			if (!ModelState.IsValid)
			{
				return 0;
			}

			operationLog.Id = Guid.NewGuid();

			return await this._operationLogRepository.AddAsync(operationLog);
		}

		[HttpPut("Edit")]
		public async Task<int> Edit(Guid? id, [Bind("Id,OperationDateTimeStart,OperationDateTimeEnd,ClientId,DiscId,OperationType,Quantity")] OperationLog operationLog)
		{
			if (id == null)
			{
				return 0;
			}

			if (id != operationLog.Id)
			{
				return 0;
			}

			try
			{
				await this._operationLogRepository.UpdateAsync(operationLog);
			}
			catch (Exception)
			{
				if (!await this._operationLogRepository.ExistsAsync(operationLog.Id))
				{
					return 0;
				}
				else
				{
					throw;
				}
			}

			return 1;
		}

		[HttpDelete("Delete")]
		public async Task<int> DeleteConfirmed(Guid id)
		{
			var client = await this._operationLogRepository.GetByIdAsync(id);

			if (client != null)
			{
				return await this._operationLogRepository.DeleteAsync(client.Id);
			}

			return 0;
		}
	}
}
