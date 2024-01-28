using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmController : Controller
    {
        private readonly IFilmRepository _filmRepository;

        public FilmController(IFilmRepository filmRepository)
        {
            this._filmRepository = filmRepository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<Film>>> GetAll(string? searchText, SortOrder sortOrder, string? sortField, int skip = 0)
        {
            var model = new IndexViewModel<Film>
            {
                SearchText = searchText,
                SortOrder = sortOrder,
                SortFieldName = sortField ?? "Id",
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
        public async Task<ActionResult<int>> Create([Bind("Id,Name,Genre,Producer,MainRole,AgeLimit")] Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                film.Id = Guid.NewGuid();

                var result = await this._filmRepository.AddAsync(film);
                if (result == 1)
                {
                    return Ok(new { Message = "Film created successfully", FilmId = film.Id });
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

        [HttpPut("Edit")]
        public async Task<ActionResult<int>> Edit(Guid? id, [Bind("Id,Name,Genre,Producer,MainRole,AgeLimit")] Film film)
        {
            if (id == null || film == null)
            {
                return BadRequest();
            }

            if (id != film.Id)
            {
                return BadRequest();
            }

            try
            {
                var result = await this._filmRepository.UpdateAsync(film);

                if (result == 1)
                {
                    return Ok(new { Message = "Film updated successfully", MusicId = film.Id });
                }
                else
                {
                    return BadRequest(new { Message = $"No records were updated. Check the provided data. Rows affected {result}" });
                }
            }
            catch (Exception ex)
            {
                if (!await this._filmRepository.ExistsAsync(film.Id))
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
            try
            {
                var film = await this._filmRepository.GetByIdAsync(id);
                if (film == null)
                {
                    return NotFound();
                }

                var result = await this._filmRepository.DeleteAsync(film.Id);

                if (result == 1)
                {
                    return Ok(new { Message = "Film deleted successfully", MusicId = id });
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