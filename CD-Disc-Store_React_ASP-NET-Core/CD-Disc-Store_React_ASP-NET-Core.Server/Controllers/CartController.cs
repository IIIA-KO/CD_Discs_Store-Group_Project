using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController(
        IDapperContext context,
        IOrderItemRepository orderItemRepository,
        IOrderRepository orderRepository,
        IOperationLogRepository operationLogRepository,
        IClientRepository clientRepository) : Controller
    {
        private readonly IDapperContext _context = context;
        private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IOperationLogRepository _operationLogRepository = operationLogRepository;
        private readonly IClientRepository _clientRepository = clientRepository;

        [HttpPost("CreateOrder")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateOrder(CartDto[] items)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();

            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (id == null)
                {
                    return BadRequest(new { Message = "User identifier not found in claims." });
                }

                var client = await _clientRepository.GetByUserIdAsync(id);

                var order = new Order
                {
                    IdClient = client.Id,
                    OperationDateTimeStart = DateTime.Now,
                    OperationDateTimeEnd = DateTime.Now
                };

                await _orderRepository.AddAsync(order, dbConnection, transaction);

                var orderItems = items
                    .GroupBy(item => item.IdDisc)
                    .Select(group => new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        IdOrder = order.Id,
                        IdDisc = group.Key,
                        Quantity = group.Sum(item => item.Quantity)
                    })
                    .ToList();

                foreach (var item in orderItems)
                {
                    await this._orderItemRepository.AddAsync(item, dbConnection, transaction);
                }

                var operationLog = new OperationLog
                {
                    IdOrder = order.Id,
                    OperationType = await _operationLogRepository.GetOperationTypeByNameAsync("Purchase")
                };

                await _operationLogRepository.AddAsync(operationLog, dbConnection, transaction);

                transaction.Commit();

                return Ok(new { Message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteOrder")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                var orderItems = await _orderItemRepository.GetByOrderIdAsync(id);
                foreach (var orderItem in orderItems)
                {
                    await _orderItemRepository.DeleteAsync(orderItem.Id);
                }

                var operationLogs = await _operationLogRepository.GetByOrderIdAsync(id);
                foreach (var operationLog in operationLogs)
                {
                    await _operationLogRepository.DeleteAsync(operationLog.Id);
                }

                var deletedOrderCount = await _orderRepository.DeleteAsync(id);

                return deletedOrderCount > 0
                    ? Ok(new { Message = "Order deleted successfully", OrderId = id })
                    : BadRequest(new { Message = "No records were deleted. Check the provided orderId." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
