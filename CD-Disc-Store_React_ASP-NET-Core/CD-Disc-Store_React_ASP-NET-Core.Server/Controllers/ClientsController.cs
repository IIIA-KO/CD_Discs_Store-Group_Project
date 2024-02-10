using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Administrator,Employee")]
    public class ClientsController(IClientRepository clientRepository) : Controller
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<Client>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
        {
            var model = new Processable<Client>
            {
                SearchText = searchText,
                SortOrder = sortOrder,
                SortFieldName = sortField ?? "id",
                Skip = skip,
                PageSize = 20
            };

            return Ok(await this._clientRepository.GetProcessedAsync(model));
        }

        [HttpGet("GetClient")]
        public async Task<ActionResult<Client>> GetClient(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var client = await this._clientRepository.GetByIdAsync(id);
                return Ok(client);
            }
            catch (Exception ex)
                when (ex is ArgumentNullException
                || ex is NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<int>> Create([FromBody] Client client)
        {
            if (!ModelState.IsValid && !await ValidateContactDetails(client))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await this._clientRepository.AddAsync(client);

                return result == 1
                    ? Ok(new { Message = "Client created successfully", ClientId = client.Id })
                    : BadRequest(new { Message = $"No records were added. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<int>> Edit([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await this._clientRepository.UpdateAsync(client);

                return result == 1
                    ? Ok(new { Message = "Client updated successfully", ClientId = client.Id })
                    : BadRequest(new { Message = $"No records were updated. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return !await this._clientRepository.ExistsAsync(client.Id)
                    ? NotFound()
                    : StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private async Task<bool> ValidateContactDetails(Client client)
        {
            var email = await this._clientRepository.GetEmailAsync(client);
            var phone = await this._clientRepository.GetPhoneAsync(client);

            var phoneAttribute = new PhoneAttribute();
            if (!phoneAttribute.IsValid(phone))
            {
                ModelState.AddModelError("Contact Phone", "Invalid phone number format.");
                return false;
            }

            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
            {
                ModelState.AddModelError("Contact Mail", "Invalid email format.");
                return false;
            }

            return true;
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<int>> DeleteConfirmed(Guid id)
        {
            try
            {
                var client = await this._clientRepository.GetByIdAsync(id);
                if (client == null)
                {
                    return NotFound();
                }

                var result = await this._clientRepository.DeleteAsync(client.Id);

                return result == 1
                    ? Ok(new { Message = "Client deleted successfully", MusicId = id })
                    : BadRequest(new { Message = $"No records were deleted. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
