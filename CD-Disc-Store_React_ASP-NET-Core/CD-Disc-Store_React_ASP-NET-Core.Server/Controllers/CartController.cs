using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController(
        IOrderItemRepository orderItemRepository,
        IOrderRepository orderRepository,
        IOperationLogRepository operationLogRepository,
        IClientRepository clientRepository) : Controller
    {
        private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IOperationLogRepository _operationLogRepository = operationLogRepository;
        private readonly IClientRepository _clientRepository = clientRepository;

        [HttpPost("CreateOrder")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateOrder(OrderItem[] orderItems)
        {
            try
            {
                int orderItemsAdded = 0;
                bool isOrderAdded = false;
                bool isOperationLogAdded = false;

                var id = User.FindFirst(ClaimTypes.NameIdentifier);

                if (id is null)
                {
                    return BadRequest(new { Message = "User identifier not found in claims." });
                }

                var client = await this._clientRepository.GetByUserIdAsync(id.Value);

                var order = new Order
                {
                    IdClient = client.Id,
                    OperationDateTimeStart = DateTime.Now,
                    OperationDateTimeEnd = DateTime.Now
                };

                if (await this._orderRepository.AddAsync(order) == 1)
                {
                    isOrderAdded = true;
                }

                foreach (var orderItem in orderItems)
                {
                    orderItem.IdOrder = order.Id;
                    if (await _orderItemRepository.AddAsync(orderItem) == 1)
                    {
                        orderItemsAdded++;
                    }
                }

                var operationLog = new OperationLog
                {
                    IdOrder = order.Id,
                    OperationType = await this._operationLogRepository.GetOperationTypeByNameAsync("Purchase")
                };

                if (await this._operationLogRepository.AddAsync(operationLog) == 1)
                {
                    isOperationLogAdded = true;
                }

                if (orderItemsAdded == 0 && !isOrderAdded && !isOperationLogAdded)
                {
                    return BadRequest(new { Message = "No records were added. Check the provided data." });
                }

                return Ok(new { Message = "Orders created successfully" });
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
                await _orderRepository.DeleteAsync((Guid)orderItem.IdOrder);
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
