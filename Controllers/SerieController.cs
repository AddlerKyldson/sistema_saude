using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_saude.Data;
using sistema_saude.Models;

namespace sistema_saude.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerieController : ControllerBase
    {
        private readonly MyDbContext _context;

        public SerieController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Series
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Serie>>> GetSerie()
        {
            return await _context.Serie.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Serie>> GetSerie(int id)
        {
            var serie = await _context.Serie.FindAsync(id);

            if (serie == null)
            {
                return NotFound();
            }

            return serie;
        }

        [HttpPost]
        public async Task<ActionResult<Serie>> PostSerie([FromBody] SerieDto serieDto)
        {
            var serie = new Serie
            {
                Nome = serieDto.Nome,
                Id_Usuario_Cadastro = serieDto.Id_Usuario_Cadastro,
                Slug = serieDto.Slug,
                Data_Cadastro = DateTime.UtcNow, // Definir a data de cadastro para agora
            };

            _context.Serie.Add(serie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSerie", new { id = serie.Id }, serie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSerie(int id, [FromBody] SerieDto serieDto)
        {
            var serie = await _context.Serie.FindAsync(id);

            if (serie == null)
            {
                return NotFound();
            }

            serie.Nome = serieDto.Nome;
            serie.Id_Usuario_Cadastro = serieDto.Id_Usuario_Cadastro;
            serie.Slug = serieDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSerie(int id)
        {
            var serie = await _context.Serie.FindAsync(id);
            if (serie == null)
            {
                return NotFound();
            }

            _context.Serie.Remove(serie);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
