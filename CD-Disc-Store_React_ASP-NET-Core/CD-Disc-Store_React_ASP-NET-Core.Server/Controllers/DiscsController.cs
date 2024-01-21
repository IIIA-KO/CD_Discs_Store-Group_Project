using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscsController : Controller
    {
        private readonly IDiscRepository _discRepository;
        private readonly ICloudStorage _cloudStorage;

        public DiscsController(IDiscRepository discRepository, ICloudStorage cloudStorage)
        {
            this._discRepository = discRepository;
            this._cloudStorage = cloudStorage;
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

            try
            {
                var disc = await this._discRepository.GetByIdAsync(id);
                return Ok(disc);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<int>> Create([Bind("Id,Name,Price,LeftOnStock,Rating,CoverImagePath,ImageStorageName,ImageFile")] Disc disc, IConfiguration configuration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (disc.ImageFile != null)
                {
                    await this.UploadFile(disc);
                }
                else
                {
                    disc.CoverImagePath = configuration.GetValue<string>("DefaultCoverImagePath");
                    disc.ImageStorageName = configuration.GetValue<string>("DefaultImageStorageName");
                }

                disc.Id = Guid.NewGuid();

                var result = await this._discRepository.AddAsync(disc);
                if (result == 1)
                {
                    return Ok(new { Message = "Disc created successfully", MusicId = disc.Id });
                }
                else
                {
                    return BadRequest(new { Message = $"No records were added. Check the provided data. Rows affected {result}" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private async Task UploadFile(Disc disc)
        {
            string fileNameForStorage = FormFileName(disc.Name, disc.ImageFile.FileName);
            disc.CoverImagePath = await this._cloudStorage.UploadFileAsync(disc.ImageFile, fileNameForStorage);
            disc.ImageStorageName = fileNameForStorage;
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{Guid.NewGuid()}{fileExtension}";
            return fileNameForStorage;
        }

        [HttpGet("GetFilmsOnDisc/{id}")]
        public async Task<ActionResult<IReadOnlyList<Film>>> GetFilms(Guid? id)
        {
            if (id == null || ! await this._discRepository.ExistsAsync(id.Value))
            {
                return NotFound();
            }

            try
            {
                var films = await this._discRepository.GetFilmsOnDiscAsync(id);
                return Ok(films);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetMusicOnDisc/{id}")]
        public async Task<ActionResult<IReadOnlyList<Music>>> GetMusic(Guid? id)
        {
            if (id == null || ! await this._discRepository.ExistsAsync(id.Value))
            {
                return NotFound();
            }

            try
            {
                var musics = await this._discRepository.GetMusicOnDiscAsync(id);
                return Ok(musics);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<int>> Edit(Guid? id, [Bind("Id,Name,Price,LeftOnStock,Rating,CoverImagePath,ImageFile,ImageStorageName")] Disc disc)
        {
            if (id == null || disc == null)
            {
                return BadRequest();
            }

            if (id != disc.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (disc.ImageFile != null)
                {
                    if (disc.ImageStorageName != null)
                    {
                        await this._cloudStorage.DeleteFileAsync(disc.ImageStorageName);
                    }

                    await UploadFile(disc);
                }

                var result = await this._discRepository.UpdateAsync(disc);

                if (result == 1)
                {
                    return Ok(new { Message = "Disc updated successfully", DiscId = disc.Id });
                }
                else
                {
                    return BadRequest(new { Message = $"No records were updated. Check the provided data. Rows affected {result}" });
                }
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
        public async Task<ActionResult<int>> DeleteConfirmed(Guid id, IConfiguration configuration)
        {
            try
            {
                var disc = await this._discRepository.GetByIdAsync(id);
                if (disc == null)
                {
                    return NotFound();
                }

                var result = await this._discRepository.DeleteAsync(disc.Id);

                if (result == 1)
                {
                    return Ok(new { Message = "Disc deleted successfully", DiscId = id });
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
