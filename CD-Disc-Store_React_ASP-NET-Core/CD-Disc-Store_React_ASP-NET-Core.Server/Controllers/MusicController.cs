using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
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

        [HttpGet("Index")]
        public async Task<ActionResult<IReadOnlyList<Music>>> Index(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
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

        [HttpGet("{id}")]
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
        public async Task<int> Create([Bind("Id,Name,Genre,Artist,Language")] Music music)
        {
            if (!ModelState.IsValid)
            {
                return 0;
            }

            music.Id = Guid.NewGuid();

            return await this._musicRepository.AddAsync(music);
        }

        [HttpPut("Edit")]
        public async Task<int> Edit(Guid? id, [Bind("Id,Name,Genre,Artist,Language")] Music music)
        {
            if (id == null)
            {
                return 0;
            }

            if (id != music.Id)
            {
                return 0;
            }
            if (music == null)
            {
                return 0;
            }

            try
            {
                await this._musicRepository.UpdateAsync(music);
            }
            catch (Exception)
            {
                if (!await this._musicRepository.ExistsAsync(music.Id))
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
            var music = await this._musicRepository.GetByIdAsync(id);

            if (music != null)
            {
                return await this._musicRepository.DeleteAsync(music.Id);
            }

            return 0;
        }
    }
}
