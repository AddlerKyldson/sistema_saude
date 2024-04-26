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
    public class Unidade_SaudeController : ControllerBase
    {
        private readonly MyDbContext _context;

        public Unidade_SaudeController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Unidade_Saudes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unidade_SaudeDto>>> GetUnidade_Saude()
        {
            var unidade_saudes = await _context
                .Unidade_Saude.Include(c => c.Bairro) // Inclui Cidade
                .ThenInclude(c => c.Cidade) // Inclui Estado
                .ThenInclude(r => r.Estado) // Inclui Estado
                .Select(c => new Unidade_SaudeDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Nome_Bairro = c.Bairro.Nome,
                    Nome_Cidade = c.Bairro.Cidade.Nome, // Corrige o acesso ao nome da cidade
                    Sigla_Estado = c.Bairro.Cidade.Estado.Sigla // Corrige o acesso à sigla do estado
                })
                .ToListAsync();

            return unidade_saudes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Unidade_SaudeDto>> GetUnidade_Saude(int id)
        {
            var unidade_saude = await _context
                .Unidade_Saude.Include(c => c.Bairro) // Inclui Bairro
                .ThenInclude(c => c.Cidade) // Inclui Cidade
                .ThenInclude(r => r.Estado) // Inclui Estado
                .FirstOrDefaultAsync(c => c.Id == id);

            if (unidade_saude == null)
            {
                return NotFound();
            }

            // Criar um DTO para formatar a resposta, incluindo as informações do estado
            var unidade_saudeDto = new Unidade_SaudeDto
            {
                Id = unidade_saude.Id,
                Nome = unidade_saude.Nome,
                Cnes = unidade_saude.Cnes,
                Tipo = unidade_saude.Tipo,
                Logradouro = unidade_saude.Logradouro,
                Id_Bairro = unidade_saude.Bairro.Id,
                Cep = unidade_saude.Cep,
                Numero = unidade_saude.Numero,
                Complemento = unidade_saude.Complemento,
                Id_Estado = unidade_saude.Bairro.Cidade.Estado.Id,
                Id_Cidade = unidade_saude.Bairro.Cidade.Id,
            };

            return unidade_saudeDto;
        }

        [HttpPost]
        public async Task<ActionResult<Unidade_Saude>> PostUnidade_Saude([FromBody] Unidade_SaudeCreateDto unidade_saudeDto)
        {
            var unidade_saude = new Unidade_Saude
            {
                Nome = unidade_saudeDto.Nome,
                Cnes = unidade_saudeDto.Cnes,
                Tipo = unidade_saudeDto.Tipo,
                Id_Bairro = unidade_saudeDto.Id_Bairro,
                Logradouro = unidade_saudeDto.Logradouro,
                Numero = unidade_saudeDto.Numero,
                Complemento = unidade_saudeDto.Complemento,
                Cep = unidade_saudeDto.Cep,
                Status = unidade_saudeDto.Status,
                Id_Usuario_Cadastro = unidade_saudeDto.Id_Usuario_Cadastro,
                Slug = unidade_saudeDto.Slug,
                Data_Cadastro = DateTime.UtcNow, // Definir a data de cadastro para agora
            };

            _context.Unidade_Saude.Add(unidade_saude);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnidade_Saude", new { id = unidade_saude.Id }, unidade_saude);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidade_Saude(int id, [FromBody] Unidade_SaudeDto unidade_saudeDto)
        {
            var unidade_saude = await _context.Unidade_Saude.FindAsync(id);

            if (unidade_saude == null)
            {
                return NotFound();
            }

            unidade_saude.Nome = unidade_saudeDto.Nome;
            unidade_saude.Id_Bairro = unidade_saudeDto.Id_Bairro;
            unidade_saude.Status = unidade_saudeDto.Status;
            unidade_saude.Data_Alteracao = DateTime.UtcNow;
            unidade_saude.Slug = unidade_saudeDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidade_Saude(int id)
        {
            var unidade_saude = await _context.Unidade_Saude.FindAsync(id);
            if (unidade_saude == null)
            {
                return NotFound();
            }

            _context.Unidade_Saude.Remove(unidade_saude);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
