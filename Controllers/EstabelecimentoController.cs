
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_saude.Data;
using sistema_saude.Models;
using Microsoft.Extensions.Logging;

namespace sistema_saude.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstabelecimentoController : ControllerBase
    {
        private readonly ILogger<EstabelecimentoController> _logger;
        private readonly MyDbContext _context;

        public EstabelecimentoController(ILogger<EstabelecimentoController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/Estabelecimentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estabelecimento>>> GetEstabelecimento([FromQuery] string? filtro_busca = null, [FromQuery] int? page = null, [FromQuery] int? perPage = null)
        {
            // Consulta básica de estabelecimentos
            var query = _context.Estabelecimento.AsQueryable();

            // Filtragem por busca, se aplicável
            if (!string.IsNullOrEmpty(filtro_busca))
            {
                query = query.Where(e => e.razao_social.Contains(filtro_busca) || e.nome_fantasia.Contains(filtro_busca) || e.cnpj.Contains(filtro_busca));
            }

            if (page != null && perPage != null)
            {
                // Verifica se a página e perPage são válidos
                if (page < 1)
                {
                    return BadRequest("Page must be greater than or equal to 1.");
                }
                if (perPage <= 0)
                {
                    return BadRequest("PerPage must be greater than 0.");
                }

                // Obtém o total de itens filtrados antes da paginação
                var total = await query.CountAsync();

                // Aplica a paginação
                var estabelecimentosFiltrados = await query
                    .OrderBy(e => e.razao_social) // Adicionando uma ordenação por razão social
                    .Skip((page.Value - 1) * perPage.Value)
                    .Take(perPage.Value)
                    .ToListAsync();

                // Retorna os dados paginados e o total de itens
                return Ok(new
                {
                    total,
                    dados = estabelecimentosFiltrados
                });
            }
            else
            {
                // Se page ou perPage forem nulos, lista todos os estabelecimentos
                var todosEstabelecimentos = await query.ToListAsync();
                return Ok(new
                {
                    total = todosEstabelecimentos.Count,
                    dados = todosEstabelecimentos
                });
            }
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
                id_tipo_estabelecimento = Estabelecimento.id_tipo_estabelecimento,
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

        [HttpGet("{id}/responsaveis-legais")]
        public async Task<ActionResult<List<Estabelecimento_Responsavel_LegalDto>>> GetResponsaveisLegais(int id)
        {
            var responsaveisLegais = await _context.Estabelecimento_Responsavel_Legal
                .Where(r => r.Id_Estabelecimento == id)
                .Select(r => new Estabelecimento_Responsavel_LegalDto
                {
                    nome_responsavel = r.Usuario.Nome,
                    CPF = r.Usuario.CPF,
                    escolaridade = r.Usuario.Escolaridade,
                    Email = r.Usuario.Email
                })
                .ToListAsync();

            if (responsaveisLegais == null || responsaveisLegais.Count == 0)
            {
                return NotFound("Nenhum responsável legal encontrado para este estabelecimento.");
            }

            return Ok(responsaveisLegais);
        }

        [HttpGet("{id}/responsaveis-tecnicos")]
        public async Task<ActionResult<List<Estabelecimento_Responsavel_TecnicoDto>>> GetResponsaveisTecnicos(int id)
        {
            var responsaveisTecnicos = await _context.Estabelecimento_Responsavel_Tecnico
                .Where(r => r.Id_Estabelecimento == id)
                .Select(r => new Estabelecimento_Responsavel_TecnicoDto
                {
                    nome_responsavel = r.Usuario.Nome,
                    CPF = r.Usuario.CPF,
                    escolaridade = r.Usuario.Escolaridade,
                    formacao = r.Usuario.Formacao,
                    especializacao = r.Usuario.Especializacao,
                    registro_conselho = r.Usuario.Registro_Conselho,
                    Email = r.Usuario.Email
                })
                .ToListAsync();

            if (responsaveisTecnicos == null || responsaveisTecnicos.Count == 0)
            {
                return NotFound("Nenhum responsável técnico encontrado para este estabelecimento.");
            }

            return Ok(responsaveisTecnicos);
        }


        [HttpPost]
        public async Task<ActionResult<Estabelecimento>> PostEstabelecimento([FromBody] EstabelecimentoDto EstabelecimentoDto)
        {

            _logger.LogInformation("Recebendo solicitação de criação de Estabelecimento.");

            if (EstabelecimentoDto.Estabelecimento_Responsavel_Legal == null)
            {
                _logger.LogInformation("Estabelecimento_Responsavel_Legal é nulo.");
                ModelState.Remove("Estabelecimento_Responsavel_Legal");
            }

            if (EstabelecimentoDto.Estabelecimento_Responsavel_Tecnico == null)
            {
                _logger.LogInformation("Estabelecimento_Responsavel_Tecnico é nulo.");
                ModelState.Remove("Estabelecimento_Responsavel_Tecnico");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido.");
                return BadRequest(ModelState);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var Estabelecimento = new Estabelecimento
                    {
                        id_tipo_estabelecimento = EstabelecimentoDto.id_tipo_estabelecimento,
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
                    Console.WriteLine(EstabelecimentoDto.Estabelecimento_Responsavel_Tecnico);



                    _logger.LogInformation("Processando responsáveis técnicos.");

                    if (EstabelecimentoDto.Estabelecimento_Responsavel_Tecnico?.Count > 0)
                    {
                        foreach (var itemDto in EstabelecimentoDto.Estabelecimento_Responsavel_Tecnico)
                        {
                            try
                            {
                                int id_usuario = await UsuarioService.VerificarResponsavelTecnicoPorCPFAsync(itemDto, _context);
                                _logger.LogInformation($"Responsável técnico processado: {id_usuario}");

                                var item = new Estabelecimento_Responsavel_Tecnico
                                {
                                    Id_Estabelecimento = Estabelecimento.id,
                                    Id_Usuario = id_usuario,
                                    Data_Cadastro = DateTime.UtcNow
                                };
                                _context.Estabelecimento_Responsavel_Tecnico.Add(item);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Erro ao processar responsável técnico: {ex.Message}");
                                throw;
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Nenhum responsável técnico encontrado.");
                    }

                    //verificar se o tamanho da lista é maior que 0
                    if (EstabelecimentoDto.Estabelecimento_Responsavel_Legal?.Count > 0)
                    {
                        _logger.LogInformation("Processando responsáveis legais.");
                        foreach (var itemDto in EstabelecimentoDto.Estabelecimento_Responsavel_Legal)
                        {
                            try
                            {
                                int id_usuario = await UsuarioService.VerificarResponsavelLegalPorCPFAsync(itemDto, _context);
                                _logger.LogInformation($"Responsável legal processado: {id_usuario}");

                                var item = new Estabelecimento_Responsavel_Legal
                                {
                                    Id_Estabelecimento = Estabelecimento.id,
                                    Id_Usuario = id_usuario,
                                    Data_Cadastro = DateTime.UtcNow
                                };
                                _context.Estabelecimento_Responsavel_Legal.Add(item);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Erro ao processar responsável legal: {ex.Message}");
                                throw;
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Nenhum responsável legal encontrado.");
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

            Estabelecimento.id_tipo_estabelecimento = EstabelecimentoDto.id_tipo_estabelecimento;
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
