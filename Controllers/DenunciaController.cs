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
    public class DenunciaController : ControllerBase
    {
        private readonly MyDbContext _context;

        public DenunciaController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Denuncias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DenunciaDto>>> GetDenuncia(
    [FromQuery] string? filtro_busca = null,
    [FromQuery] int page = 1,
    [FromQuery] int perPage = 20
)
        {
            // Verifica se a página é válida
            if (page < 1)
            {
                return BadRequest("Page must be greater than or equal to 1.");
            }

            // Consulta básica de Denuncia com o relacionamento de Bairro
            var query = _context.Denuncia
                                .AsQueryable();

            // Filtragem por busca, se aplicável
            if (!string.IsNullOrEmpty(filtro_busca))
            {
                query = query.Where(d =>
                    d.Status == 1  // Apenas registros ativos
                    && (
                    d.Descricao.Contains(filtro_busca)
                    || d.Texto_Denuncia.Contains(filtro_busca)
                    || d.Bairro.Contains(filtro_busca)
                    )
                );
            }
            else
            {
                query = query.Where(d =>
                    d.Status == 1  // Apenas registros ativos
                );
            }

            // Ordenar por data de cadastro (descendente)
            query = query.OrderByDescending(d => d.Data_Cadastro);

            // Paginação
            var denunciasFiltradas = await query
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .Select(denuncia => new DenunciaDto
                {
                    Id = denuncia.Id,
                    Descricao = denuncia.Descricao,
                    Data_Recebimento = denuncia.Data_Recebimento,
                    Bairro = denuncia.Bairro,
                    Tipo_Denuncia = denuncia.Tipo_Denuncia,
                    Origem_Denuncia = denuncia.Origem_Denuncia,
                    Forma_Recebimento = denuncia.Forma_Recebimento,
                    Id_Usuario = denuncia.Id_Usuario,
                    Escolaridade = denuncia.Escolaridade,
                    Texto_Denuncia = denuncia.Texto_Denuncia,
                    Atendida = denuncia.Atendida,
                    Orgao_Atendimento = denuncia.Orgao_Atendimento,
                    Data_Atendimento = denuncia.Data_Atendimento,
                    Orgao_Encaminhamento = denuncia.Orgao_Encaminhamento,
                    Data_Encaminhamento = denuncia.Data_Encaminhamento,
                    Motivo_Nao_Atendimento = denuncia.Motivo_Nao_Atendimento,
                    Status = denuncia.Status,
                    Id_Usuario_Cadastro = denuncia.Id_Usuario_Cadastro,
                    Id_Usuario_Alteracao = denuncia.Id_Usuario_Alteracao,
                    Data_Cadastro = denuncia.Data_Cadastro,
                    Data_Alteracao = denuncia.Data_Alteracao,
                })
                .ToListAsync();

            var total = await query.CountAsync();

            return Ok(new { total, dados = denunciasFiltradas });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DenunciaDto>> GetDenuncia(int id)
        {
            var Denuncia = await _context.Denuncia
                .Include(c => c.Cidade)
                        .ThenInclude(c => c.Estado)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Denuncia == null)
            {
                return NotFound();
            }

            var DenunciaDto = new DenunciaDto
            {
                Id = Denuncia.Id,
                Descricao = Denuncia.Descricao,
                Data_Recebimento = Denuncia.Data_Recebimento,
                Bairro = Denuncia.Bairro,
                Id_Cidade = Denuncia.Id_Cidade,
                Id_Estado = Denuncia.Cidade.Estado.Sigla,
                Tipo_Denuncia = Denuncia.Tipo_Denuncia,
                Origem_Denuncia = Denuncia.Origem_Denuncia,
                Forma_Recebimento = Denuncia.Forma_Recebimento,
                Id_Usuario = Denuncia.Id_Usuario,
                Escolaridade = Denuncia.Escolaridade,
                Texto_Denuncia = Denuncia.Texto_Denuncia,
                Atendida = Denuncia.Atendida,
                Orgao_Atendimento = Denuncia.Orgao_Atendimento,
                Data_Atendimento = Denuncia.Data_Atendimento,
                Orgao_Encaminhamento = Denuncia.Orgao_Encaminhamento,
                Data_Encaminhamento = Denuncia.Data_Encaminhamento,
                Motivo_Nao_Atendimento = Denuncia.Motivo_Nao_Atendimento,
                Status = Denuncia.Status,
                Id_Usuario_Cadastro = Denuncia.Id_Usuario_Cadastro,
                Id_Usuario_Alteracao = Denuncia.Id_Usuario_Alteracao,
                Data_Cadastro = Denuncia.Data_Cadastro,
                Data_Alteracao = Denuncia.Data_Alteracao,

            };



            return DenunciaDto;
        }

        [HttpPost]
        public async Task<ActionResult<Denuncia>> PostDenuncia(
            [FromBody] DenunciaDto denunciaDto
        )
        {
            var denuncia = new Denuncia
            {
                Descricao = denunciaDto.Descricao,
                Data_Recebimento = denunciaDto.Data_Recebimento,
                Bairro = denunciaDto.Bairro,
                Id_Cidade = denunciaDto.Id_Cidade,
                Tipo_Denuncia = denunciaDto.Tipo_Denuncia,
                Origem_Denuncia = denunciaDto.Origem_Denuncia,
                Forma_Recebimento = denunciaDto.Forma_Recebimento,
                Id_Usuario = denunciaDto.Id_Usuario,
                Escolaridade = denunciaDto.Escolaridade,
                Texto_Denuncia = denunciaDto.Texto_Denuncia,
                Atendida = denunciaDto.Atendida,
                Orgao_Atendimento = denunciaDto.Orgao_Atendimento,
                Data_Atendimento = denunciaDto.Data_Atendimento,
                Orgao_Encaminhamento = denunciaDto.Orgao_Encaminhamento,
                Data_Encaminhamento = denunciaDto.Data_Encaminhamento,
                Motivo_Nao_Atendimento = denunciaDto.Motivo_Nao_Atendimento,
                Status = 1,
                Id_Usuario_Cadastro = denunciaDto.Id_Usuario_Cadastro,
                Data_Cadastro = DateTime.UtcNow,
            };

            _context.Denuncia.Add(denuncia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDenuncia", new { id = denuncia.Id }, denuncia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDenuncia(
            int id,
            [FromBody] DenunciaDto denunciaDto
        )
        {
            var denuncia = await _context.Denuncia.FindAsync(id);

            if (denuncia == null)
            {
                return NotFound();
            }

            denuncia.Descricao = denunciaDto.Descricao;
            denuncia.Data_Recebimento = denunciaDto.Data_Recebimento;
            denuncia.Bairro = denunciaDto.Bairro;
            denuncia.Id_Cidade = denunciaDto.Id_Cidade;
            denuncia.Tipo_Denuncia = denunciaDto.Tipo_Denuncia;
            denuncia.Origem_Denuncia = denunciaDto.Origem_Denuncia;
            denuncia.Forma_Recebimento = denunciaDto.Forma_Recebimento;
            denuncia.Id_Usuario = denunciaDto.Id_Usuario;
            denuncia.Escolaridade = denunciaDto.Escolaridade;
            denuncia.Texto_Denuncia = denunciaDto.Texto_Denuncia;
            denuncia.Atendida = denunciaDto.Atendida;
            denuncia.Orgao_Atendimento = denunciaDto.Orgao_Atendimento;
            denuncia.Data_Atendimento = denunciaDto.Data_Atendimento;
            denuncia.Orgao_Encaminhamento = denunciaDto.Orgao_Encaminhamento;
            denuncia.Data_Encaminhamento = denunciaDto.Data_Encaminhamento;
            denuncia.Motivo_Nao_Atendimento = denunciaDto.Motivo_Nao_Atendimento;
            denuncia.Status = 1;
            denuncia.Id_Usuario_Alteracao = denunciaDto.Id_Usuario_Alteracao;
            denuncia.Data_Alteracao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDenuncia(int id)
        {
            //atualiza o status para 0
            var denuncia = await _context.Denuncia.FindAsync(id);

            if (denuncia == null)
            {
                return NotFound();
            }

            denuncia.Status = 0;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
