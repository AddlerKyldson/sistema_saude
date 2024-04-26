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
    public class BairroController : ControllerBase
    {
        private readonly MyDbContext _context;

        public BairroController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Bairros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BairroDto>>> GetBairro()
        {
            var bairros = await _context
                .Bairro.Include(c => c.Cidade) // Inclui Cidade
                .ThenInclude(r => r.Estado)
                .Select(c => new BairroDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Nome_Cidade = c.Cidade.Nome,
                    Sigla_Estado = c.Cidade.Estado.Sigla // Corrige o acesso à sigla do estado
                })
                .ToListAsync();

            return bairros;
        }

        [HttpGet("Cidade/{idCidade}")]
        public async Task<ActionResult<IEnumerable<Bairro>>> GetBairrosPorCidade(int idCidade)
        {
            var bairros = await _context.Bairro
                                        .Where(r => r.Id_Cidade == idCidade)
                                        .ToListAsync();

            if (bairros.Count == 0)
            {
                //retornar um array vazio
                return bairros;
            }

            return bairros;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BairroDto>> GetBairro(int id)
        {
            var bairro = await _context
                .Bairro.Include(c => c.Cidade)
                .ThenInclude(r => r.Estado)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (bairro == null)
            {
                return NotFound();
            }

            // Criar um DTO para formatar a resposta, incluindo as informações do estado
            var bairroDto = new BairroDto
            {
                Id = bairro.Id,
                Nome = bairro.Nome,
                Nome_Cidade = bairro.Cidade.Nome,
                Id_Cidade = bairro.Cidade.Id,
                Id_Estado = bairro.Cidade.Estado.Id,
            };

            return bairroDto;
        }

        [HttpPost]
        public async Task<ActionResult<Bairro>> PostBairro([FromBody] BairroCreateDto bairroDto)
        {
            var bairro = new Bairro
            {
                Nome = bairroDto.Nome,
                Id_Cidade = bairroDto.Id_Cidade,
                Status = bairroDto.Status,
                Id_Usuario_Cadastro = bairroDto.Id_Usuario_Cadastro,
                Slug = bairroDto.Slug,
                Data_Cadastro = DateTime.UtcNow, // Definir a data de cadastro para agora
            };

            _context.Bairro.Add(bairro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBairro", new { id = bairro.Id }, bairro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBairro(int id, [FromBody] BairroDto bairroDto)
        {
            var bairro = await _context.Bairro.FindAsync(id);

            if (bairro == null)
            {
                return NotFound();
            }

            bairro.Nome = bairroDto.Nome;
            bairro.Id_Cidade = bairroDto.Id_Cidade;
            bairro.Status = bairroDto.Status;
            bairro.Data_Alteracao = DateTime.UtcNow;
            bairro.Slug = bairroDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBairro(int id)
        {
            var bairro = await _context.Bairro.FindAsync(id);
            if (bairro == null)
            {
                return NotFound();
            }

            _context.Bairro.Remove(bairro);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
