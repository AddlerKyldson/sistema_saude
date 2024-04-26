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
    public class RegionalController : ControllerBase
    {
        private readonly MyDbContext _context;

        public RegionalController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Regionals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionalDto>>> GetRegional()
        {
            var regioes = await _context
                .Regional.Include(r => r.Estado)
                .Select(r => new RegionalDto
                {
                    Id = r.Id,
                    Nome = r.Nome,
                    SiglaEstado = r.Estado.Sigla
                })
                .ToListAsync();

            return regioes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Regional>> GetRegional(int id)
        {
            var regional = await _context.Regional.FindAsync(id);

            if (regional == null)
            {
                return NotFound();
            }

            return regional;
        }

        [HttpGet("Estado/{idEstado}")]
        public async Task<ActionResult<IEnumerable<Regional>>> GetRegionaisPorEstado(int idEstado)
        {
            var regionais = await _context.Regional
                                        .Where(r => r.Id_Estado == idEstado)
                                        .ToListAsync();

            if (regionais.Count == 0)
            {
                //retornar um array vazio
                return regionais;
            }

            return regionais;
        }

        [HttpPost]
        public async Task<ActionResult<Regional>> PostRegional(
            [FromBody] RegionalCreateDto regionalDto
        )
        {
            var regional = new Regional
            {
                Nome = regionalDto.Nome,
                Id_Estado = regionalDto.Id_Estado,
                Status = regionalDto.Status,
                Id_Usuario_Cadastro = regionalDto.Id_Usuario_Cadastro,
                Slug = regionalDto.Slug,
                Data_Cadastro = DateTime.UtcNow, // Definir a data de cadastro para agora
            };

            _context.Regional.Add(regional);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegional", new { id = regional.Id }, regional);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegional(
            int id,
            [FromBody] RegionalUpdateDto regionalUpdateDto
        )
        {
            var regional = await _context.Regional.FindAsync(id);

            if (regional == null)
            {
                return NotFound();
            }

            regional.Nome = regionalUpdateDto.Nome;
            regional.Id_Estado = regionalUpdateDto.Id_Estado;
            regional.Status = regionalUpdateDto.Status;
            regional.Data_Alteracao = DateTime.UtcNow;
            regional.Slug = regionalUpdateDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegional(int id)
        {
            var regional = await _context.Regional.FindAsync(id);
            if (regional == null)
            {
                return NotFound();
            }

            _context.Regional.Remove(regional);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
