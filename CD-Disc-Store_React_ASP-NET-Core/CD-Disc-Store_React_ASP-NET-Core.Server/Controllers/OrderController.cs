using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Administrator,Employee")]
    public class OrderController(IOrderRepository orderRepository) : Controller
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
        {
            var model = new Processable<Order>
            {
                SearchText = searchText,
                SortOrder = sortOrder,
                SortFieldName = sortField?.ToLowerInvariant() ?? "id",
                Skip = skip,
                PageSize = 20
            };

            return Ok(await this._orderRepository.GetProcessedAsync(model));
        }

        [HttpGet("GetOrder")]
        public async Task<ActionResult<Order>> GetOrder(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var order = await this._orderRepository.GetByIdAsync(id);
                return Ok(order);
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
                var order = await this._orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                var result = await this._orderRepository.DeleteAsync(order.Id);

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
