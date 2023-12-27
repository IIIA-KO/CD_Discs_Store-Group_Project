using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
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

        [HttpGet("Index")]
        public async Task<ActionResult<IReadOnlyList<Film>>> Index()
        {
            return Ok(await this._filmRepository.GetAllAsync());
        }

        [HttpPost("Create")]
        public async Task<int> Create([Bind("Id,Name,Genre,Producer,MainRole,AgeLimit")] Film film)
        {
            if(!ModelState.IsValid)
            {
                return 0;
            }

            film.Id = Guid.NewGuid();

            return await this._filmRepository.AddAsync(film);
        }

        [HttpPut("Edit")]
        public async Task<int> Edit(Guid? id, [Bind("Id,Name,Genre,Producer,MainRole,AgeLimit")] Film film)
        {
            if (id == null)
            {
                return 0;
            }

            if (id != film.Id)
            {
                return 0;
            }

            try
            {
                await this._filmRepository.UpdateAsync(film);
            }
            catch (Exception)
            {
                if (!await this._filmRepository.ExistsAsync(film.Id))
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
            var client = await this._filmRepository.GetByIdAsync(id);

            if (client != null)
            {
                return await this._filmRepository.DeleteAsync(client.Id);
            }

            return 0;
        }
    }
}
