using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IReadOnlyList<Film>>> GetAll()
        {
            return Ok(await this._filmRepository.GetAllAsync());
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

                return Ok(await this._filmRepository.AddAsync(film));
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
                return Ok(await this._filmRepository.UpdateAsync(film));
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
            var film = await this._filmRepository.GetByIdAsync(id);

            return film == null ? NotFound()
                : Ok(await this._filmRepository.DeleteAsync(film.Id));
        }
    }
}