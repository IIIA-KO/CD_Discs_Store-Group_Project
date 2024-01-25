using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderItemController : Controller
	{
		private readonly IOrderItemRepository _orderItemRepository;

		public OrderItemController(IOrderItemRepository orderItemRepository)
		{
			this._orderItemRepository = orderItemRepository;
		}

		[HttpGet("GetAll")]
		public async Task<ActionResult<IReadOnlyList<OrderItem>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
		{
			var model = new IndexViewModel<OrderItem>
			{
				SearchText = searchText,
				SortOrder = sortOrder,
				SortFieldName = sortField ?? "Id",
				Skip = skip,
				CountItems = await this._orderItemRepository.CountProcessedDataAsync(searchText),
				PageSize = 20
			};

			return Ok(model.Items = await this._orderItemRepository.GetProcessedAsync(
						model.SearchText,
						model.SortOrder,
						model.SortFieldName,
						model.Skip,
						model.PageSize));
		}

		[HttpGet("GetOrderItem")]
		public async Task<ActionResult<OrderItem>> GetDisc(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			try
			{
				var music = await this._orderItemRepository.GetByIdAsync(id);
				return Ok(music);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
		}

		[HttpPost("Create")]
		public async Task<ActionResult<int>> Create([Bind("Id,IdOrder,IdDisc,Quantity")] OrderItem orderItem)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				orderItem.Id = Guid.NewGuid();

				var result = await this._orderItemRepository.AddAsync(orderItem);
				if (result == 1)
				{
					return Ok(new { Message = "Order item created successfully", MusicId = orderItem.Id });
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
		public async Task<ActionResult<int>> Edit(Guid? id, [Bind("Id,Name,Genre,Artist,Language")] OrderItem orderItem)
		{
			if (id == null || orderItem == null)
			{
				return BadRequest();
			}

			if (id != orderItem.Id)
			{
				return BadRequest();
			}

			try
			{
				var result = await this._orderItemRepository.UpdateAsync(orderItem);

				if (result == 1)
				{
					return Ok(new { Message = "Order item updated successfully", MusicId = orderItem.Id });
				}
				else
				{
					return BadRequest(new { Message = $"No records were updated. Check the provided data. Rows affected {result}" });
				}
			}
			catch (Exception ex)
			{
				if (!await this._orderItemRepository.ExistsAsync(orderItem.Id))
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
				var music = await this._orderItemRepository.GetByIdAsync(id);
				if (music == null)
				{
					return NotFound();
				}

				var result = await this._orderItemRepository.DeleteAsync(music.Id);

				if (result == 1)
				{
					return Ok(new { Message = "Order item deleted successfully", MusicId = id });
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