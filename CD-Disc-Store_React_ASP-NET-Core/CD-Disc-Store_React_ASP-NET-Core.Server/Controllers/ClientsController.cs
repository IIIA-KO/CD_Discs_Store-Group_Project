using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            this._clientRepository = clientRepository;
        }

        [HttpGet("Index")]
        public async Task<ActionResult<IReadOnlyList<Client>>> Index(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
        {
            var model = new IndexViewModel<Client>
            {
                SearchText = searchText,
                SortOrder = sortOrder,
                SortFieldName = sortField ?? "Id",
                Skip = skip,
                CountItems = await this._clientRepository.CountProcessedDataAsync(searchText),
                PageSize = 20
            };

            return Ok(model.Items = await this._clientRepository.GetProcessedAsync(
                        model.SearchText,
                        model.SortOrder,
                        model.SortFieldName,
                        model.Skip,
                        model.PageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await this._clientRepository.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPost("Create")]
        public async Task<int> Create([Bind("Id,FirstName,LastName,Address,City,ContactPhone,ContactMail,BirthDay,MarriedStatus,Sex,HasChild")] Client client)
        {
            if (!ModelState.IsValid && !ValidateContactDetails(client))
            {
                return 0;
            }

            client.Id = Guid.NewGuid();

            return await this._clientRepository.AddAsync(client);
        }

        [HttpPut("Edit")]
        public async Task<int> Edit(Guid? id, [Bind("Id,FirstName,LastName,Address,City,ContactPhone,ContactMail,BirthDay,MarriedStatus,Sex,HasChild")] Client client)
        {
            if(id == null)
            {
                return 0;
            }

            if(id != client.Id)
            {
                return 0;
            }

            try
            {
                await this._clientRepository.UpdateAsync(client);
            }
            catch(Exception)
            {
                if(!await this._clientRepository.ExistsAsync(client.Id))
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

        private bool ValidateContactDetails(Client client)
        {
            if (!Regex.IsMatch(client.ContactPhone, PhoneValidation.PhonePattern))
            {
                ModelState.AddModelError("Contact Phone", "Contact phone does not match the pattern: 'xx-xxx-xx-xx'");
                return false;
            }

            if (!Regex.IsMatch(client.ContactMail, EmailAddressValidation.EmailPattern))
            {
                ModelState.AddModelError("Contact Mail", "Contact mail does not match the pattern: 'user@example.com'");
                return false;
            }

            return true;
        }

        [HttpDelete("Delete")]
        public async Task<int> DeleteConfirmed(Guid id)
        {
            var client = await this._clientRepository.GetByIdAsync(id);

            if (client != null)
            {
                return await this._clientRepository.DeleteAsync(client.Id);
            }

            return 0;
        }
    }
}