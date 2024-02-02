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
    public class FilmController(IFilmRepository filmRepository, ICloudStorage cloudStorage) : Controller
    {
        private readonly IFilmRepository _filmRepository = filmRepository;
        private readonly ICloudStorage _cloudStorage = cloudStorage;

        [HttpGet("GetAll")]
        [Authorize(Roles = "Administrator, Employee, Client")]
        public async Task<ActionResult<IReadOnlyList<Film>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
        {
            var model = new ProcessableViewModel<Film>
            {
                SearchText = searchText,
                SortOrder = sortOrder,
                SortFieldName = sortField?.ToLowerInvariant() ?? "id",
                Skip = skip,
                PageSize = 20
            };

            return Ok(await this._filmRepository.GetProcessedAsync(model));
        }

        [HttpGet("GetFilm")]
        [Authorize(Roles = "Administrator, Employee, Client")]
        public async Task<ActionResult<Film>> GetFilm(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var film = await this._filmRepository.GetByIdAsync(id);
                return Ok(film);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

        [HttpPost("Create")]
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<ActionResult<int>> Create([Bind("Id,Name,Genre,Producer,MainRole,AgeLimit,CoverImagePath,ImageStorageName,ImageFile")] Film film, StorageOptions storageOptions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (film.ImageFile != null)
                {
                    if (!await this._cloudStorage.UploadFileAsync(film))
                    {
                        return BadRequest(new { Message = $"Couldn't upload {typeof(Film).Name} cover to storage. No records were added to database." });
                    }
                }
                else
                {
                    film.CoverImagePath = storageOptions.GetDefaultFilmCoverImagePath(film);
                    film.ImageStorageName = storageOptions.GetDefaultFilmImageStorageName(film);
                }

                var result = await this._filmRepository.AddAsync(film);

                return result == 1
                    ? Ok(new { Message = "Film created successfully", FilmId = film.Id })
                    : BadRequest(new { Message = $"No records were added. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("Edit")]
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<ActionResult<int>> Edit(Guid? existingFilmId, [Bind("Id,Name,Genre,Producer,MainRole,AgeLimit,CoverImagePath,ImageStorageName,ImageFile")] Film changed, StorageOptions storageOptions)
        {
            if (!existingFilmId.HasValue || changed == null)
            {
                return BadRequest(new { Message = "Failed to update film. Invalid Id was provided." });
            }

            if (!ModelState.IsValid || existingFilmId.Value != changed.Id)
            {
                ModelState.AddModelError("Id", "Failed to update film. Existing Film's Id is not equal to Changed Film's Id");
                return BadRequest(ModelState);
            }

            try
            {
                var existing = await this._filmRepository.GetByIdAsync(existingFilmId.Value);

                if (changed.ImageFile != null)
                {
                    if (!string.Equals(existing.ImageStorageName, storageOptions.GetDefaultFilmImageStorageName(existing), StringComparison.OrdinalIgnoreCase))
                    {
                        await this._cloudStorage.DeleteFileAsync(existing.ImageStorageName);
                    }

                    await this._cloudStorage.UploadFileAsync(changed);
                }

                var result = await this._filmRepository.UpdateAsync(changed);

                return result == 1
                    ? Ok(new { Message = "Film updated successfully", MusicId = changed.Id })
                    : BadRequest(new { Message = $"No records were updated. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return !await this._filmRepository.ExistsAsync(changed.Id)
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
                var film = await this._filmRepository.GetByIdAsync(id);
                if (film == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(film.ImageStorageName)
                    && film.ImageStorageName != storageOptions.GetDefaultFilmImageStorageName(film))
                {
                    await this._cloudStorage.DeleteFileAsync(film.ImageStorageName);
                }

                var result = await this._filmRepository.DeleteAsync(film.Id);

                return result == 1
                    ? Ok(new { Message = "Film deleted successfully", MusicId = id })
                    : BadRequest(new { Message = $"No records were deleted. Check the provided data. Rows affected {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
