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
    public class EstadoController : ControllerBase
    {
        private readonly MyDbContext _context;

        public EstadoController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Estados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estado>>> GetEstado()
        {
            return await _context.Estado.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estado>> GetEstado(int id)
        {
            var estado = await _context.Estado.FindAsync(id);

            if (estado == null)
            {
                return NotFound();
            }

            return estado;
        }

        [HttpPost]
        public async Task<ActionResult<Estado>> PostEstado([FromBody] EstadoDto estadoDto)
        {
            var estado = new Estado
            {
                Nome = estadoDto.Nome,
                Sigla = estadoDto.Sigla,
                Id_Usuario_Cadastro = estadoDto.Id_Usuario_Cadastro,
                Slug = estadoDto.Slug,
                Data_Cadastro = DateTime.UtcNow, // Definir a data de cadastro para agora
            };

            _context.Estado.Add(estado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstado", new { id = estado.Id }, estado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstado(int id, [FromBody] EstadoDto estadoDto)
        {
            var estado = await _context.Estado.FindAsync(id);

            if (estado == null)
            {
                return NotFound();
            }

            estado.Nome = estadoDto.Nome;
            estado.Sigla = estadoDto.Sigla;
            estado.Id_Usuario_Cadastro = estadoDto.Id_Usuario_Cadastro;
            estado.Slug = estadoDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstado(int id)
        {
            var estado = await _context.Estado.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            _context.Estado.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
