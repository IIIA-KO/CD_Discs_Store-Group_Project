using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Options;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscsController(IDiscRepository discRepository, ICloudStorage cloudStorage) : Controller
    {
        private readonly IDiscRepository _discRepository = discRepository;
        private readonly ICloudStorage _cloudStorage = cloudStorage;

        [HttpGet("GetAll")]
        [Authorize(Roles = "Administrator, Employee, Client")]
        public async Task<ActionResult<IReadOnlyList<Disc>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
        {
			var model = new ProcessableViewModel<Disc>
			{
				SearchText = searchText,
				SortOrder = sortOrder,
                SortFieldName = sortField?.ToLowerInvariant() ?? "id",
                Skip = skip,
				PageSize = pageSize
			};

			return Ok(await this._discRepository.GetProcessedAsync(model));
		}

        [HttpGet("GetDisc")]
        [Authorize(Roles = "Administrator, Employee, Client")]
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
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<ActionResult<int>> Create([Bind("Id,Name,Price,LeftOnStock,Rating,CoverImagePath,ImageStorageName,ImageFile")] Disc disc, StorageOptions storageOptions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (disc.ImageFile != null)
                {
                    if (!await this._cloudStorage.UploadFileAsync(disc))
                    {
                        return BadRequest(new { Message = $"Couldn't upload {typeof(Disc).Name} cover to storage. No records were added to database." });
                    }
                }
                else
                {
                    disc.CoverImagePath = storageOptions.DefaultDiscCoverImagePath;
                    disc.ImageStorageName = storageOptions.DefaultDiscImageStorageName;
                }

                var result = await this._discRepository.AddAsync(disc);
                return result == 1
                    ? Ok(new { Message = "Disc created successfully", MusicId = disc.Id })
                    : BadRequest(new { Message = $"No records were added. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetFilmsOnDisc/{id}")]
        [Authorize(Roles = "Administrator, Employee, Client")]
        public async Task<ActionResult<IReadOnlyList<Film>>> GetFilms(Guid? id)
        {
            if (id == null || !await this._discRepository.ExistsAsync(id.Value))
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
        [Authorize(Roles = "Administrator, Employee, Client")]
        public async Task<ActionResult<IReadOnlyList<Music>>> GetMusic(Guid? id)
        {
            if (id == null || !await this._discRepository.ExistsAsync(id.Value))
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
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<ActionResult<int>> Edit(Guid? existingDiscId, [Bind("Id,Name,Price,LeftOnStock,Rating,CoverImagePath,ImageFile,ImageStorageName")] Disc changed, StorageOptions storageOptions)
        {
            if (!existingDiscId.HasValue || changed == null)
            {
                return BadRequest(new { Message = "Failed to update disc. Invalid Id was provided." });
            }

            if (!ModelState.IsValid || existingDiscId.Value != changed.Id)
            {
                ModelState.AddModelError("Id", "Failed to update disc. Existing Disc's Id is not equal to Changed Disc's Id");
                return BadRequest(ModelState);
            }

            try
            {
                var existing = await this._discRepository.GetByIdAsync(existingDiscId.Value);

                if (changed.ImageFile != null)
                {
                    if (existing.ImageStorageName != null)
                    {
                        await this._cloudStorage.DeleteFileAsync(existing.ImageStorageName);
                    }

                    if (!string.Equals(existing.ImageStorageName, storageOptions.DefaultDiscImageStorageName, StringComparison.OrdinalIgnoreCase))
                    {
                        await this._cloudStorage.DeleteFileAsync(existing.ImageStorageName);
                    }

                    await this._cloudStorage.UploadFileAsync(changed);
                }

                var result = await this._discRepository.UpdateAsync(changed);

                return result == 1
                    ? Ok(new { Message = "Disc updated successfully", DiscId = changed.Id })
                    : BadRequest(new { Message = $"No records were updated. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return !await this._discRepository.ExistsAsync(changed.Id)
                    ? NotFound()
                    : StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<ActionResult<int>> DeleteConfirmed(Guid id, StorageOptions storageOptions)
        {
            try
            {
                var disc = await this._discRepository.GetByIdAsync(id);
                if (disc == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(disc.ImageStorageName)
                    && disc.ImageStorageName != storageOptions.DefaultDiscImageStorageName)
                {
                    await this._cloudStorage.DeleteFileAsync(disc.ImageStorageName);
                }

                var result = await this._discRepository.DeleteAsync(disc.Id);

                return result == 1
                    ? Ok(new { Message = "Disc deleted successfully", DiscId = id })
                    : BadRequest(new { Message = $"No records were deleted. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
