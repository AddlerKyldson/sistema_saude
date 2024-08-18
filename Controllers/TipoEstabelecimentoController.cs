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
    public class Tipo_EstabelecimentoController : ControllerBase
    {
        private readonly MyDbContext _context;

        public Tipo_EstabelecimentoController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Tipo_Estabelecimentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipo_EstabelecimentoDto>>> GetTipo_Estabelecimento([FromQuery] string? filtro_busca = null, [FromQuery] int page = 1, [FromQuery] int perPage = 20)
        {
            // Verifica se a página é válida
            if (page < 1)
            {
                return BadRequest("Page must be greater than or equal to 1.");
            }

            // Consulta básica de tipo de estabelecimentos
            var query = _context.Tipo_Estabelecimento.AsQueryable();

            // Filtragem por busca, se aplicável
            if (!string.IsNullOrEmpty(filtro_busca))
            {
                query = query.Where(te => te.Nome.Contains(filtro_busca) || te.Slug.Contains(filtro_busca));
            }

            // Obtém o total de itens filtrados
            var total = await query.CountAsync();

            // Paginação
            var tipoEstabelecimentosFiltrados = await query
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();

            // Retorna os dados e o total de itens
            return Ok(new
            {
                total,
                dados = tipoEstabelecimentosFiltrados
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tipo_Estabelecimento>> GetTipo_Estabelecimento(int id)
        {
            var tipoEstabelecimentos = await _context.Tipo_Estabelecimento.FindAsync(id);

            if (tipoEstabelecimentos == null)
            {
                return NotFound();
            }

            return tipoEstabelecimentos;
        }

        [HttpPost]
        public async Task<ActionResult<Tipo_Estabelecimento>> PostTipo_Estabelecimento(
            [FromBody] Tipo_EstabelecimentoDto tipoEstabelecimentoDto
        )
        {
            var tipoEstabelecimento = new Tipo_Estabelecimento
            {
                Nome = tipoEstabelecimentoDto.Nome,
                Id_Serie = tipoEstabelecimentoDto.Id_Serie,
                Status = tipoEstabelecimentoDto.Status,
                Id_Usuario_Cadastro = tipoEstabelecimentoDto.Id_Usuario_Cadastro,
                Slug = tipoEstabelecimentoDto.Slug,
                Data_Cadastro = DateTime.UtcNow, // Definir a data de cadastro para agora
            };

            _context.Tipo_Estabelecimento.Add(tipoEstabelecimento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipo_Estabelecimento", new { id = tipoEstabelecimento.Id }, tipoEstabelecimento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipo_Estabelecimento(
            int id,
            [FromBody] Tipo_EstabelecimentoDto tipoEstabelecimentoUpdateDto
        )
        {
            var tipoEstabelecimento = await _context.Tipo_Estabelecimento.FindAsync(id);

            if (tipoEstabelecimento == null)
            {
                return NotFound();
            }

            tipoEstabelecimento.Nome = tipoEstabelecimentoUpdateDto.Nome;
            tipoEstabelecimento.Id_Serie = tipoEstabelecimentoUpdateDto.Id_Serie;
            tipoEstabelecimento.Status = tipoEstabelecimentoUpdateDto.Status;
            tipoEstabelecimento.Data_Alteracao = DateTime.UtcNow;
            tipoEstabelecimento.Slug = tipoEstabelecimentoUpdateDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo_Estabelecimento(int id)
        {
            var tipoEstabelecimento = await _context.Tipo_Estabelecimento.FindAsync(id);
            if (tipoEstabelecimento == null)
            {
                return NotFound();
            }

            _context.Tipo_Estabelecimento.Remove(tipoEstabelecimento);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
