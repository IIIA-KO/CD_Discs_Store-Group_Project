using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicController : Controller
    {
        private readonly IMusicRepository _musicRepository;

        public MusicController(IMusicRepository musicRepository)
        {
            this._musicRepository = musicRepository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<Music>>> GetAll()
        {
            return Ok(await this._musicRepository.GetAllAsync());
        }

        [HttpGet("GetDisc")]
        public async Task<ActionResult<Music>> GetDisc(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var music = await this._musicRepository.GetByIdAsync(id);

            if (music == null)
            {
                return NotFound();
            }

            return Ok(music);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<int>> Create([Bind("Id,Name,Genre,Artist,Language")] Music music)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            music.Id = Guid.NewGuid();

            return Ok(await this._musicRepository.AddAsync(music));
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<int>> Edit(Guid? id, [Bind("Id,Name,Genre,Artist,Language")] Music music)
        {
            if (id == null || music == null)
            {
                return BadRequest();
            }

            if (id != music.Id)
            {
                return BadRequest();
            }

            try
            {
                return Ok(await this._musicRepository.UpdateAsync(music));
            }
            catch (Exception ex)
            {
                if (!await this._musicRepository.ExistsAsync(music.Id))
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
            var music = await this._musicRepository.GetByIdAsync(id);

            return music == null ? NotFound()
                : Ok(await this._musicRepository.DeleteAsync(music.Id));
        }
    }
}