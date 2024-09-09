using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_saude.Data;
using sistema_saude.Models;

namespace sistema_saude.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspecaoController : ControllerBase
    {
        private readonly MyDbContext _context;

        public InspecaoController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Inspecaos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InspecaoDto>>> GetInspecao(
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

            // Consulta básica de Inspecao com Estabelecimento e Usuario relacionado
            var query = _context.Inspecao
                                .Include(i => i.Estabelecimento)
                                .Include(i => i.Usuario)  // Incluindo o usuário responsável técnico
                                .AsQueryable();

            // Filtragem por busca, se aplicável
            if (!string.IsNullOrEmpty(filtro_busca))
            {
                query = query.Where(te =>
                    te.Status == 1  // Apenas registros ativos
                    && (
                    te.Descricao.Contains(filtro_busca)
                    || te.Slug.Contains(filtro_busca)
                    || te.Estabelecimento.razao_social.Contains(filtro_busca)
                    || te.Estabelecimento.nome_fantasia.Contains(filtro_busca)
                    || te.Estabelecimento.cnpj.Contains(filtro_busca)
                    || te.Usuario.Nome.Contains(filtro_busca)
                    )  // Filtragem por nome do usuário responsável
                );
            }
            else
            {
                query = query.Where(te =>
                    te.Status == 1  // Apenas registros ativos
                );
            }

            // Ordenar por última data de inspeção (descendente)
            query = query.OrderByDescending(te => te.Data_inspecao);

            // Paginação
            var inspecoesFiltrados = await query
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .Select(inspecao => new InspecaoDto
                {
                    Id = inspecao.Id,
                    Descricao = inspecao.Descricao,
                    N_termo_inspecao = inspecao.N_termo_inspecao,
                    Data_inspecao = inspecao.Data_inspecao,
                    Motivo_inspecao = inspecao.Motivo_inspecao,
                    Verificacao_Restaurantes = inspecao.Verificacao_Restaurantes,
                    Verificacao_Supermercados = inspecao.Verificacao_Supermercados,
                    Verificacao_Escolas = inspecao.Verificacao_Escolas,
                    Verificacao_Veiculos_Alimentos = inspecao.Verificacao_Veiculos_Alimentos,
                    Estabelecimento = inspecao.Estabelecimento == null ? null : new EstabelecimentoDto
                    {
                        id = inspecao.Estabelecimento.id,
                        razao_social = inspecao.Estabelecimento.razao_social,
                        nome_fantasia = inspecao.Estabelecimento.nome_fantasia,
                        cnpj = inspecao.Estabelecimento.cnpj
                    },
                    Responsavel_Tecnico = inspecao.Usuario == null ? null : new UsuarioDto  // Dados do usuário responsável
                    {
                        Id = inspecao.Usuario.Id,
                        Nome = inspecao.Usuario.Nome,
                        Email = inspecao.Usuario.Email
                    }
                })
                .ToListAsync();

            var total = await query.CountAsync();

            return Ok(new { total, dados = inspecoesFiltrados });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inspecao>> GetInspecao(int id)
        {
            var inspecoes = await _context.Inspecao.FindAsync(id);

            if (inspecoes == null)
            {
                return NotFound();
            }

            return inspecoes;
        }

        [HttpPost]
        public async Task<ActionResult<Inspecao>> PostInspecao([FromBody] InspecaoDto inspecaoDto)
        {
            var inspecao = new Inspecao
            {
                Descricao = inspecaoDto.Descricao,
                N_termo_inspecao = inspecaoDto.N_termo_inspecao,
                Data_inspecao = inspecaoDto.Data_inspecao,
                Motivo_inspecao = inspecaoDto.Motivo_inspecao,
                Id_estabelecimento = inspecaoDto.Id_estabelecimento,
                Id_responsavel_tecnico = inspecaoDto.Id_responsavel_tecnico,
                Roteiro_Inspecao = inspecaoDto.Roteiro_Inspecao,

                Verificacao_Restaurantes = inspecaoDto.Verificacao_Restaurantes,
                Verificacao_Supermercados = inspecaoDto.Verificacao_Supermercados,
                Verificacao_Escolas = inspecaoDto.Verificacao_Escolas,
                Verificacao_Veiculos_Alimentos = inspecaoDto.Verificacao_Veiculos_Alimentos,

                Status = inspecaoDto.Status,
                Id_Usuario_Cadastro = inspecaoDto.Id_Usuario_Cadastro,
                Data_Cadastro = DateTimeOffset.Now,
                Slug = inspecaoDto.Slug
            };

            _context.Inspecao.Add(inspecao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInspecao", new { id = inspecao.Id }, inspecao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInspecao(
            int id,
            [FromBody] InspecaoDto inspecaoUpdateDto
        )
        {
            var inspecao = await _context.Inspecao.FindAsync(id);

            if (inspecao == null)
            {
                return NotFound();
            }

            inspecao.Descricao = inspecaoUpdateDto.Descricao;
            inspecao.N_termo_inspecao = inspecaoUpdateDto.N_termo_inspecao;
            inspecao.Data_inspecao = inspecaoUpdateDto.Data_inspecao;
            inspecao.Motivo_inspecao = inspecaoUpdateDto.Motivo_inspecao;
            inspecao.Id_estabelecimento = inspecaoUpdateDto.Id_estabelecimento;
            inspecao.Id_responsavel_tecnico = inspecaoUpdateDto.Id_responsavel_tecnico;
            inspecao.Roteiro_Inspecao = inspecaoUpdateDto.Roteiro_Inspecao;
            inspecao.Verificacao_Restaurantes = inspecaoUpdateDto.Verificacao_Restaurantes;
            inspecao.Verificacao_Supermercados = inspecaoUpdateDto.Verificacao_Supermercados;
            inspecao.Verificacao_Escolas = inspecaoUpdateDto.Verificacao_Escolas;
            inspecao.Verificacao_Veiculos_Alimentos = inspecaoUpdateDto.Verificacao_Veiculos_Alimentos;

            inspecao.Status = inspecaoUpdateDto.Status;
            inspecao.Id_Usuario_Alteracao = inspecaoUpdateDto.Id_Usuario_Alteracao;
            inspecao.Data_Alteracao = DateTimeOffset.Now;
            inspecao.Slug = inspecaoUpdateDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspecao(int id)
        {
            //Apenas altere o campo status para 0
            var inspecao = await _context.Inspecao.FindAsync(id);

            if (inspecao == null)
            {
                return NotFound();
            }

            inspecao.Status = 0;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
