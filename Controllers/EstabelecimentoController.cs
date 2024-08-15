
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_saude.Data;
using sistema_saude.Models;

namespace sistema_saude.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstabelecimentoController : ControllerBase
    {
        private readonly MyDbContext _context;

        public EstabelecimentoController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Estabelecimentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estabelecimento>>> GetEstabelecimento([FromQuery] string? filtro_busca = null, [FromQuery] int page = 1, [FromQuery] int perPage = 20)
        {
            // Verifica se a página é válida
            if (page < 1)
            {
                return BadRequest("Page must be greater than or equal to 1.");
            }

            // Consulta básica de medicamentos
            var query = _context.Estabelecimento.AsQueryable();

            // Filtragem por busca, se aplicável
            if (!string.IsNullOrEmpty(filtro_busca))
            {
                query = query.Where(m => m.razao_social.Contains(filtro_busca) || m.nome_fantasia.Contains(filtro_busca) || m.cnpj.Contains(filtro_busca));
            }

            // Obtém o total de itens filtrados
            var total = await query.CountAsync();

            // Paginação
            var estabelecimentosFiltrados = await query
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();

            // Retorna os dados e o total de itens
            return Ok(new
            {
                total,
                dados = estabelecimentosFiltrados
            });

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstabelecimentoDto>> GetEstabelecimento(int id)
        {
            var Estabelecimento = await _context.Estabelecimento
                .Include(c => c.Bairro)
                    .ThenInclude(b => b.Cidade)
                        .ThenInclude(c => c.Estado)
                .FirstOrDefaultAsync(c => c.id == id);

            if (Estabelecimento == null)
            {
                return NotFound();
            }

            var EstabelecimentoDto = new EstabelecimentoDto
            {
                id = Estabelecimento.id,
                razao_social = Estabelecimento.razao_social,
                nome_fantasia = Estabelecimento.nome_fantasia,
                cnpj = Estabelecimento.cnpj,
                cnae = Estabelecimento.cnae,
                data_inicio_funcionamento = Estabelecimento.data_inicio_funcionamento,
                grau_risco = Estabelecimento.grau_risco,
                inscricao_municipal = Estabelecimento.inscricao_municipal,
                inscricao_estadual = Estabelecimento.inscricao_estadual,
                logradouro = Estabelecimento.logradouro,
                id_bairro = Estabelecimento.id_bairro,
                id_cidade = Estabelecimento.Bairro.Cidade.Id,
                id_estado = Estabelecimento.Bairro.Cidade.Estado.Id,
                cep = Estabelecimento.cep,
                complemento = Estabelecimento.complemento,
                telefone = Estabelecimento.telefone,
                email = Estabelecimento.email,
                protocolo_funcionamento = Estabelecimento.protocolo_funcionamento,
                passivo_alvara_sanitario = Estabelecimento.passivo_alvara_sanitario,
                n_alvara_sanitario = Estabelecimento.n_alvara_sanitario,
                coleta_residuos = Estabelecimento.coleta_residuos,
                autuacao_visa = Estabelecimento.autuacao_visa,
                forma_abastecimento = Estabelecimento.forma_abastecimento,
                status = Estabelecimento.status,
                id_usuario_cadastro = Estabelecimento.id_usuario_cadastro,
                data_cadastro = Estabelecimento.data_cadastro,
                slug = Estabelecimento.slug,
            };

            return EstabelecimentoDto;
        }

        [HttpPost]
        public async Task<ActionResult<Estabelecimento>> PostEstabelecimento([FromBody] EstabelecimentoDto EstabelecimentoDto)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var Estabelecimento = new Estabelecimento
                    {
                        razao_social = EstabelecimentoDto.razao_social,
                        nome_fantasia = EstabelecimentoDto.nome_fantasia,
                        cnpj = EstabelecimentoDto.cnpj,
                        cnae = EstabelecimentoDto.cnae,
                        data_inicio_funcionamento = EstabelecimentoDto.data_inicio_funcionamento,
                        grau_risco = EstabelecimentoDto.grau_risco,
                        inscricao_municipal = EstabelecimentoDto.inscricao_municipal,
                        inscricao_estadual = EstabelecimentoDto.inscricao_estadual,
                        logradouro = EstabelecimentoDto.logradouro,
                        id_bairro = EstabelecimentoDto.id_bairro,
                        cep = EstabelecimentoDto.cep,
                        complemento = EstabelecimentoDto.complemento,
                        telefone = EstabelecimentoDto.telefone,
                        email = EstabelecimentoDto.email,
                        protocolo_funcionamento = EstabelecimentoDto.protocolo_funcionamento,
                        passivo_alvara_sanitario = EstabelecimentoDto.passivo_alvara_sanitario,
                        n_alvara_sanitario = EstabelecimentoDto.n_alvara_sanitario,
                        coleta_residuos = EstabelecimentoDto.coleta_residuos,
                        autuacao_visa = EstabelecimentoDto.autuacao_visa,
                        forma_abastecimento = EstabelecimentoDto.forma_abastecimento,
                        status = EstabelecimentoDto.status,
                        id_usuario_cadastro = EstabelecimentoDto.id_usuario_cadastro,
                        data_cadastro = DateTime.UtcNow,
                        slug = EstabelecimentoDto.slug
                    };

                    _context.Estabelecimento.Add(Estabelecimento);
                    await _context.SaveChangesAsync();

                    Console.WriteLine("Vai listar agora.");
                    Console.WriteLine(EstabelecimentoDto.Estabelecimento_Responsavel_Legal);

                    foreach (var itemDto in EstabelecimentoDto.Estabelecimento_Responsavel_Legal)
                    {

                        Console.WriteLine("listando responsáveis.");

                        int id_usuario = await UsuarioService.VerificarUsuarioPorCPFAsync(itemDto, _context);

                        var item = new Estabelecimento_Responsavel_Legal
                        {
                            Id_Estabelecimento = Estabelecimento.id,
                            Id_Usuario = id_usuario,
                            Data_Cadastro = DateTime.UtcNow
                        };
                        _context.Estabelecimento_Responsavel_Legal.Add(item);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction("GetEstabelecimento", new { id = Estabelecimento.id }, Estabelecimento);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return BadRequest(e.Message);
                }
            }


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstabelecimento(int id, [FromBody] EstabelecimentoDto EstabelecimentoDto)
        {
            var Estabelecimento = await _context.Estabelecimento.FindAsync(id);

            if (Estabelecimento == null)
            {
                return NotFound();
            }

            Estabelecimento.razao_social = EstabelecimentoDto.razao_social;
            Estabelecimento.nome_fantasia = EstabelecimentoDto.nome_fantasia;
            Estabelecimento.cnpj = EstabelecimentoDto.cnpj;
            Estabelecimento.cnae = EstabelecimentoDto.cnae;
            Estabelecimento.data_inicio_funcionamento = EstabelecimentoDto.data_inicio_funcionamento;
            Estabelecimento.grau_risco = EstabelecimentoDto.grau_risco;
            Estabelecimento.inscricao_municipal = EstabelecimentoDto.inscricao_municipal;
            Estabelecimento.inscricao_estadual = EstabelecimentoDto.inscricao_estadual;
            Estabelecimento.logradouro = EstabelecimentoDto.logradouro;
            Estabelecimento.id_bairro = EstabelecimentoDto.id_bairro;
            Estabelecimento.cep = EstabelecimentoDto.cep;
            Estabelecimento.complemento = EstabelecimentoDto.complemento;
            Estabelecimento.telefone = EstabelecimentoDto.telefone;
            Estabelecimento.email = EstabelecimentoDto.email;
            Estabelecimento.protocolo_funcionamento = EstabelecimentoDto.protocolo_funcionamento;
            Estabelecimento.passivo_alvara_sanitario = EstabelecimentoDto.passivo_alvara_sanitario;
            Estabelecimento.n_alvara_sanitario = EstabelecimentoDto.n_alvara_sanitario;
            Estabelecimento.coleta_residuos = EstabelecimentoDto.coleta_residuos;
            Estabelecimento.autuacao_visa = EstabelecimentoDto.autuacao_visa;
            Estabelecimento.forma_abastecimento = EstabelecimentoDto.forma_abastecimento;
            Estabelecimento.status = EstabelecimentoDto.status;
            Estabelecimento.id_usuario_cadastro = EstabelecimentoDto.id_usuario_cadastro;
            Estabelecimento.data_cadastro = EstabelecimentoDto.data_cadastro;
            Estabelecimento.slug = EstabelecimentoDto.slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstabelecimento(int id)
        {
            var Estabelecimento = await _context.Estabelecimento.FindAsync(id);
            if (Estabelecimento == null)
            {
                return NotFound();
            }

            _context.Estabelecimento.Remove(Estabelecimento);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
