using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IOperationLogRepository operationLogRepository) : Controller
    {
        private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IOperationLogRepository _operationLogRepository = operationLogRepository;

        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CartOrderViewModel model)
        {
            try
            {
                int orderItemsAdded = 0;
                int ordersAdded = 0;
                int operationLogsAdded = 0;

                foreach (var orderItem in model.OrderItems)
                {
                    if (await _orderItemRepository.AddAsync(orderItem) == 1)
                    {
                        orderItemsAdded++;
                    }
                }

                foreach (var order in model.Orders)
                {
                    if (await _orderRepository.AddAsync(order) == 1)
                    {
                        ordersAdded++;
                    }
                }

                foreach (var operation in model.OperationLogs)
                {
                    if (await _operationLogRepository.AddAsync(operation) == 1)
                    {
                        operationLogsAdded++;
                    }
                }

                if (orderItemsAdded + ordersAdded + operationLogsAdded == 0)
                {
                    return BadRequest(new { Message = "No records were added. Check the provided data." });
                }

                return Ok(new
                {
                    Message = "Orders created successfully",
                    OrderItemsAdded = orderItemsAdded,
                    OrdersAdded = ordersAdded,
                    OperationLogsAdded = operationLogsAdded
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteOrder")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> DeleteOrder(Guid? id)
        {
            try
            {
                var orderItem = await _orderItemRepository.GetByIdAsync(id);

                var processable = new Processable<OperationLog>
                {
                    SearchText = $"IdOrder = {orderItem.IdOrder}",
                };

                var operationLog = _operationLogRepository.GetProcessedAsync(processable).Result.FirstOrDefault();

                if (operationLog != null)
                {
                    await _operationLogRepository.DeleteAsync(operationLog.Id);
                }
                await _orderRepository.DeleteAsync(orderItem.IdOrder);
                var result = await this._orderItemRepository.DeleteAsync(orderItem.Id);

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
