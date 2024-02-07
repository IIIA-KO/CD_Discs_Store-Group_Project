using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Administrator,Employee")]
    public class MusicController(IMusicRepository musicRepository, ICloudStorage cloudStorage) : Controller
    {
        private readonly IMusicRepository _musicRepository = musicRepository;
        private readonly ICloudStorage _cloudStorage = cloudStorage;

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<Music>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
        {
            var model = new Processable<Music>
            {
                SearchText = searchText,
                SortOrder = sortOrder,
                SortFieldName = sortField?.ToLowerInvariant() ?? "id",
                Skip = skip,
                PageSize = 20
            };

            return Ok(await this._musicRepository.GetProcessedAsync(model));
        }

        [HttpGet("GetMusic")]
        [AllowAnonymous]
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
        public async Task<ActionResult<int>> Create([Bind("Id,Name,Genre,Artist,Language,CoverImagePath,ImageStorageName,ImageFile")] Music music, StorageOptions storageOptions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (music.ImageFile != null)
                {
                    if (!await this._cloudStorage.UploadFileAsync(music))
                    {
                        return BadRequest(new { Message = $"Couldn't upload {typeof(Music).Name} cover to storage. No records were added to database." });
                    }
                }
                else
                {
                    music.CoverImagePath = storageOptions.GetDefaultMusicCoverImagePath(music);
                    music.ImageStorageName = storageOptions.GetDefaultMusicImageStorageName(music);
                }

                var result = await this._musicRepository.AddAsync(music);

                return result == 1
                    ? Ok(new { Message = "Music created successfully", MusicId = music.Id })
                    : BadRequest(new { Message = $"No records were added. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<int>> Edit(Guid? existingMusicId, [Bind("Id,Name,Genre,Artist,Language,CoverImagePath,ImageStorageName,ImageFile")] Music changed, StorageOptions storageOptions)
        {
            if (!existingMusicId.HasValue || changed == null)
            {
                return BadRequest(new { Message = "Failed to update music. Invalid Id was provided." });
            }

            if (!ModelState.IsValid || existingMusicId.Value != changed.Id)
            {
                ModelState.AddModelError("Id", "Failed to update music. Existing Music Id is not equal to Changed Music Id");
                return BadRequest(ModelState);
            }

            try
            {
                var existing = await this._musicRepository.GetByIdAsync(existingMusicId.Value);

                if (changed.ImageFile != null)
                {
                    if (!string.Equals(existing.ImageStorageName, storageOptions.GetDefaultMusicImageStorageName(existing), StringComparison.OrdinalIgnoreCase))
                    {
                        await this._cloudStorage.DeleteFileAsync(existing.ImageStorageName);
                    }

                    await this._cloudStorage.UploadFileAsync(changed);
                }

                var result = await this._musicRepository.UpdateAsync(changed);

                return result == 1
                    ? Ok(new { Message = "Music updated successfully", MusicId = changed.Id })
                    : BadRequest(new { Message = $"No records were updated. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return !await this._musicRepository.ExistsAsync(changed.Id)
                    ? NotFound()
                    : StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<int>> DeleteConfirmed(Guid id, StorageOptions storageOptions)
        {
            try
            {
                var music = await this._musicRepository.GetByIdAsync(id);
                if (music == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(music.ImageStorageName)
                    && music.ImageStorageName != storageOptions.GetDefaultMusicImageStorageName(music))
                {
                    await this._cloudStorage.DeleteFileAsync(music.ImageStorageName);
                }

                var result = await this._musicRepository.DeleteAsync(music.Id);

                return result == 1
                    ? Ok(new { Message = "Music deleted successfully", MusicId = id })
                    : BadRequest(new { Message = $"No records were deleted. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
