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
    public class CidadeController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CidadeController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Cidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CidadeDto>>> GetCidade()
        {
            var cidades = await _context.Cidade
                .Include(c => c.Regional) // Inclui Regional
                .Include(c => c.Estado) // Inclui Estado diretamente
                .Select(c => new CidadeDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Nome_Regional = c.Regional != null ? c.Regional.Nome : null, // Verifica se Regional existe
                    Sigla_Estado = c.Estado.Sigla // Acessa diretamente a sigla do Estado
                })
                .ToListAsync();

            return cidades;
        }

        [HttpGet("Estado/{idEstado}")]
        public async Task<ActionResult<IEnumerable<Cidade>>> GetCidadesPorEstado(string idEstado)
        {
            // Verificar se o estado foi encontrado
            var estado = await _context.Estado
                                       .FirstOrDefaultAsync(e => e.Sigla == idEstado);

            if (estado == null)
            {
                return NotFound("Estado não encontrado.");
            }

            // Buscar as cidades associadas ao estado encontrado
            var cidades = await _context.Cidade
                                        .Where(c => c.Id_Estado == estado.Id)
                                        .Select(c => new
                                        {
                                            c.Id,
                                            c.Codigo_IBGE,
                                            c.Nome,
                                            c.Id_Regional,
                                            c.Id_Estado,
                                        })
                                        .ToListAsync();

            // Retornar a lista de cidades como JSON
            return Ok(cidades);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CidadeDto>> GetCidade(int id)
        {
            var cidade = await _context
                .Cidade.Include(c => c.Regional)
                .ThenInclude(r => r.Estado)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cidade == null)
            {
                return NotFound();
            }

            // Criar um DTO para formatar a resposta, incluindo as informações do estado
            var cidadeDto = new CidadeDto
            {
                Id = cidade.Id,
                Nome = cidade.Nome,
                Nome_Regional = cidade.Regional.Nome,
                Id_Regional = cidade.Regional.Id,
                Id_Estado = cidade.Regional.Estado.Id
            };

            return cidadeDto;
        }

        [HttpPost]
        public async Task<ActionResult<Cidade>> PostCidade([FromBody] CidadeCreateDto cidadeDto)
        {
            var cidade = new Cidade
            {
                Nome = cidadeDto.Nome,
                Id_Regional = cidadeDto.Id_Regional,
                Id_Estado = cidadeDto.Id_Estado,
                Status = cidadeDto.Status,
                Id_Usuario_Cadastro = cidadeDto.Id_Usuario_Cadastro,
                Slug = cidadeDto.Slug,
                Data_Cadastro = DateTime.UtcNow, // Definir a data de cadastro para agora
            };

            _context.Cidade.Add(cidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCidade", new { id = cidade.Id }, cidade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCidade(int id, [FromBody] CidadeDto cidadeDto)
        {
            var cidade = await _context.Cidade.FindAsync(id);

            if (cidade == null)
            {
                return NotFound();
            }

            cidade.Nome = cidadeDto.Nome;
            cidade.Id_Regional = cidadeDto.Id_Regional;
            cidade.Id_Estado = cidadeDto.Id_Estado;
            cidade.Status = cidadeDto.Status;
            cidade.Data_Alteracao = DateTime.UtcNow;
            cidade.Slug = cidadeDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCidade(int id)
        {
            var cidade = await _context.Cidade.FindAsync(id);
            if (cidade == null)
            {
                return NotFound();
            }

            _context.Cidade.Remove(cidade);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
