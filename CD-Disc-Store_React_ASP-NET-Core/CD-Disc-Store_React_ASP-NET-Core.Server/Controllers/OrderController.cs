using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
	{
		private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        [HttpGet("GetAll")]
		public async Task<ActionResult<IReadOnlyList<Order>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
		{
			var model = new IndexViewModel<Order>
			{
				SearchText = searchText,
				SortOrder = sortOrder,
				SortFieldName = sortField ?? "Id",
				Skip = skip,
				CountItems = await this._orderRepository.CountProcessedDataAsync(searchText),
				PageSize = 20
			};

			return Ok(model.Items = await this._orderRepository.GetProcessedAsync(
						model.SearchText,
						model.SortOrder,
						model.SortFieldName,
						model.Skip,
						model.PageSize));
		}

		[HttpGet("GetOrder")]
		public async Task<ActionResult<Order>> GetDisc(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			try
			{
				var music = await this._orderRepository.GetByIdAsync(id);
				return Ok(music);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
		}

		[HttpPost("Create")]
		public async Task<ActionResult<int>> Create([Bind("Id,OrderDateTime,IdClient")] Order order)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				order.Id = Guid.NewGuid();

				var result = await this._orderRepository.AddAsync(order);
				if (result == 1)
				{
					return Ok(new { Message = "Order created successfully", MusicId = order.Id });
				}
				else
				{
					return BadRequest(new { Message = $"No records were added. Check the provided data. Rows affected {result}" });
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal Server Error: {ex.Message}");
			}
		}

		[HttpPut("Edit")]
		public async Task<ActionResult<int>> Edit(Guid? id, [Bind("Id,OrderDateTime,IdClient")] Order order)
		{
			if (id == null || order == null)
			{
				return BadRequest();
			}

			if (id != order.Id)
			{
				return BadRequest();
			}

			try
			{
				var result = await this._orderRepository.UpdateAsync(order);

				if (result == 1)
				{
					return Ok(new { Message = "Order updated successfully", MusicId = order.Id });
				}
				else
				{
					return BadRequest(new { Message = $"No records were updated. Check the provided data. Rows affected {result}" });
				}
			}
			catch (Exception ex)
			{
				if (!await this._orderRepository.ExistsAsync(order.Id))
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
			try
			{
				var music = await this._orderRepository.GetByIdAsync(id);
				if (music == null)
				{
					return NotFound();
				}

				var result = await this._orderRepository.DeleteAsync(music.Id);

				if (result == 1)
				{
					return Ok(new { Message = "Order deleted successfully", MusicId = id });
				}
				else
				{
					return BadRequest(new { Message = $"No records were deleted. Check the provided data. Rows affected {result}" });
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal Server Error: {ex.Message}");
			}
		}
	}
}
