using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Options;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services.Interfaces;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Administrator")]
    public class FilmController : Controller
    {
        private readonly IFilmRepository _filmRepository;
        private readonly ICloudStorage _cloudStorage;

        public FilmController(IFilmRepository filmRepository, ICloudStorage cloudStorage)
        {
            this._filmRepository = filmRepository;
            this._cloudStorage = cloudStorage;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<Film>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
        {
            var model = new GetAllViewModel<Film>
            {
                SearchText = searchText,
                SortOrder = sortOrder,
                SortFieldName = sortField?.ToLowerInvariant() ?? "id",
                Skip = skip,
                CountItems = await this._filmRepository.CountProcessedDataAsync(searchText),
                PageSize = 20
            };

            return Ok(model.Items = await this._filmRepository.GetProcessedAsync(
                        model.SearchText,
                        model.SortOrder,
                        model.SortFieldName,
                        model.Skip,
                        model.PageSize));
        }

        [HttpGet("GetFilm")]
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

                film.Id = Guid.NewGuid();

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
        public async Task<ActionResult<int>> Edit(Guid? existingFilmId, [Bind("Id,Name,Genre,Producer,MainRole,AgeLimit,CoverImagePath,ImageStorageName,ImageFile")] Film changed)
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
                    if (existing.ImageStorageName != null)
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
