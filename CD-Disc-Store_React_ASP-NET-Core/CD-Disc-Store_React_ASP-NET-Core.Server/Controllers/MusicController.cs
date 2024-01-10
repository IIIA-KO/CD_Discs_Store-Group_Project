using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


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
        public async Task<ActionResult<IReadOnlyList<Music>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
        {
            var model = new IndexViewModel<Music>
            {
                SearchText = searchText,
                SortOrder = sortOrder,
                SortFieldName = sortField ?? "Id",
                Skip = skip,
                CountItems = await this._musicRepository.CountProcessedDataAsync(searchText),
                PageSize = 20
            };

            return Ok(model.Items = await this._musicRepository.GetProcessedAsync(
                        model.SearchText,
                        model.SortOrder,
                        model.SortFieldName,
                        model.Skip,
                        model.PageSize));
        }

        [HttpGet("GetDisc")]
        public async Task<ActionResult<Music>> GetDisc(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var music = await this._musicRepository.GetByIdAsync(id);
                return Ok(music);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<int>> Create([Bind("Id,Name,Genre,Artist,Language")] Music music)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                music.Id = Guid.NewGuid();

                return Ok(await this._musicRepository.AddAsync(music));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
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