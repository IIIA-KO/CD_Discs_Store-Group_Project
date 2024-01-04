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

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<Disc>>> GetAll()
        {
            return Ok(await this._discRepository.GetAllAsync());
        }

        [HttpGet("GetDisc")]
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
        public async Task<ActionResult<int>> Create([Bind("Id,Name,Price,LeftOnStock,Rating,CoverImagePath")] Disc disc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            disc.Id = Guid.NewGuid();

            return Ok(await this._discRepository.AddAsync(disc));
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<int>> Edit(Guid? id, [Bind("Id,Name,Price,LeftOnStock,Rating,CoverImagePath")] Disc disc)
        {
            if (id == null || disc == null)
            {
                return BadRequest();
            }

            if (id != disc.Id)
            {
                return BadRequest();
            }

            try
            {
                return Ok(await this._discRepository.UpdateAsync(disc));
            }
            catch (Exception ex)
            {
                if (!await this._discRepository.ExistsAsync(disc.Id))
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
            var disc = await this._discRepository.GetByIdAsync(id);

            return disc == null ? NotFound() 
                : Ok(await this._discRepository.DeleteAsync(disc.Id));
        }
    }
}