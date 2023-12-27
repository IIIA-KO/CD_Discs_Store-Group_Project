using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscsController : Controller
    {
        private readonly IDiscRepository _discRepository;

        public DiscsController(IDiscRepository discRepository)
        {
            this._discRepository = discRepository;
        }

        [HttpGet("Index")]
        public async Task<ActionResult<IReadOnlyList<Disc>>> Index()
        {
            return Ok(await this._discRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Disc>> GetDisc(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disc = await this._discRepository.GetByIdAsync(id);

            if (disc == null)
            {
                return NotFound();
            }

            return Ok(disc);
        }

        [HttpPost("Create")]
        public async Task<int> Create([Bind("Id,Name,Price,LeftOnStock,Rating")] Disc disc)
        {
            if (!ModelState.IsValid)
            {
                return 0;
            }

            disc.Id = Guid.NewGuid();

            return await this._discRepository.AddAsync(disc);
        }

        [HttpPut("Edit")]
        public async Task<int> Edit(Guid? id, [Bind("Id,Name,Price,LeftOnStock,Rating")] Disc disc)
        {
            if (id == null)
            {
                return 0;
            }

            if (id != disc.Id)
            {
                return 0;
            }
            if (disc == null)
            {
                return 0;
            }

            try
            {
                await this._discRepository.UpdateAsync(disc);
            }
            catch (Exception)
            {
                if (!await this._discRepository.ExistsAsync(disc.Id))
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
            var disc = await this._discRepository.GetByIdAsync(id);

            if (disc != null)
            {
                return await this._discRepository.DeleteAsync(disc.Id);
            }

            return 0;
        }
    }
}
