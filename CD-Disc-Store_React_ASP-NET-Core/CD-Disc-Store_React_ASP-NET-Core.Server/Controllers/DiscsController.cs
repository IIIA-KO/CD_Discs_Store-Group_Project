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

                return Ok(await this._discRepository.AddAsync(disc));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private async Task UploadFile(Disc disc)
        {
            string fileNameForStorage = FormFileName(disc.Name, disc.ImageFile.FileName);
            disc.CoverImagePath = await _cloudStorage.UploadFileAsync(disc.ImageFile, fileNameForStorage);
            disc.ImageStorageName = fileNameForStorage;
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{Guid.NewGuid()}{fileExtension}";
            return fileNameForStorage;
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
        public async Task<ActionResult<int>> DeleteConfirmed(Guid id, IConfiguration configuration)
        {
            var disc = await this._discRepository.GetByIdAsync(id);

            if (disc is null)
            {
                return NotFound();
            }

            if (disc.ImageStorageName != null && disc.ImageStorageName != configuration.GetValue<string>("DefaultImageStorageName"))
            {
                await this._cloudStorage.DeleteFileAsync(disc.ImageStorageName);
            }

            return Ok(await this._discRepository.DeleteAsync(disc.Id));
        }
    }
}