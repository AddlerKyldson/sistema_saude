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
    public class Medicamento_MovimentacaoController : ControllerBase
    {
        private readonly MyDbContext _context;

        public Medicamento_MovimentacaoController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicamento_Movimentacao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicamento_Movimentacao>>> GetMedicamento_Movimentacao([FromQuery] string tipo, [FromQuery] string? filtro_busca = null, [FromQuery] int page = 1, [FromQuery] int perPage = 20)
        {
            // Verifica se a página é válida
            if (page < 1)
            {
                return BadRequest("Page must be greater than or equal to 1.");
            }

            // Consulta básica de medicamentos
            var query = _context.Medicamento_Movimentacao.AsQueryable();

            // Filtragem por busca, se aplicável
            if (!string.IsNullOrEmpty(filtro_busca))
            {
                query = query.Where(m => m.Descricao.Contains(filtro_busca));
            }

            // Filtragem por tipo, se aplicável
            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(m => m.Tipo == int.Parse(tipo));
            }

            // Obtém o total de itens filtrados
            var total = await query.CountAsync();

            // Paginação
            var medicamentosFiltrados = await query
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();

            // Retorna os dados e o total de itens
            return Ok(new
            {
                total,
                dados = medicamentosFiltrados
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento_Movimentacao>> GetMedicamento_Movimentacao(
            int id
        )
        {
            var medicamento_movimentacao = await _context.Medicamento_Movimentacao.FindAsync(id);

            if (medicamento_movimentacao == null)
            {
                return NotFound();
            }

            return medicamento_movimentacao;
        }

        [HttpPost]
        public async Task<ActionResult<Medicamento_Movimentacao>> PostMedicamento_Movimentacao(
            [FromBody] Medicamento_MovimentacaoDto medicamento_movimentacaoDto
        )
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var medicamento_movimentacao = new Medicamento_Movimentacao
                    {
                        Descricao = medicamento_movimentacaoDto.Descricao,
                        Data = medicamento_movimentacaoDto.Data,
                        Tipo = medicamento_movimentacaoDto.Tipo,
                        Id_Usuario_Cadastro = medicamento_movimentacaoDto.Id_Usuario_Cadastro,
                        Data_Cadastro = DateTime.UtcNow,
                    };

                    _context.Medicamento_Movimentacao.Add(medicamento_movimentacao);
                    await _context.SaveChangesAsync();

                    foreach (
                        var itemDto in medicamento_movimentacaoDto.Medicamento_Movimentacao_Item
                    )
                    {

                        await MedicamentoService.VerificarMedicamentoPorCodigoBarrasAsync(itemDto.Codigo_Barras, itemDto.Nome, itemDto.Quantidade, medicamento_movimentacaoDto.Tipo, _context);

                        var item = new Medicamento_Movimentacao_Item
                        {
                            Id_Medicamento_Movimentacao = medicamento_movimentacao.Id,
                            Id_Medicamento = itemDto.Codigo_Barras,
                            Quantidade = itemDto.Quantidade,
                        };
                        _context.Medicamento_Movimentacao_Item.Add(item);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction(
                        "GetMedicamento_Movimentacao",
                        new { id = medicamento_movimentacao.Id },
                        medicamento_movimentacao
                    );
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Ocorreu um erro: " + ex.Message);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicamento_Movimentacao(
            int id,
            [FromBody] Medicamento_MovimentacaoDto medicamento_movimentacaoDto
        )
        {
            var medicamento_movimentacao = await _context.Medicamento_Movimentacao.FindAsync(id);

            if (medicamento_movimentacao == null)
            {
                return NotFound();
            }

            medicamento_movimentacao.Descricao = medicamento_movimentacaoDto.Descricao;
            medicamento_movimentacao.Data = medicamento_movimentacaoDto.Data;
            medicamento_movimentacao.Tipo = medicamento_movimentacaoDto.Tipo;
            medicamento_movimentacao.Id_Usuario_Cadastro =
                medicamento_movimentacaoDto.Id_Usuario_Cadastro;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicamento_Movimentacao(int id)
        {
            try
            {
                Console.WriteLine($"Iniciando exclusão de Medicamento_Movimentacao com Id: {id}");

                var medicamento_movimentacao_item = await _context.Medicamento_Movimentacao_Item
                    .Where(x => x.Id_Medicamento_Movimentacao == id)
                    .ToListAsync();

                Console.WriteLine($"Itens de movimentação carregados: {medicamento_movimentacao_item.Count}");

                var medicamento_movimentacao = await _context.Medicamento_Movimentacao.FindAsync(id);
                if (medicamento_movimentacao == null)
                {
                    Console.WriteLine($"Medicamento_Movimentacao com Id: {id} não encontrado");
                    return NotFound();
                }

                foreach (var item in medicamento_movimentacao_item)
                {
                    try
                    {
                        Console.WriteLine($"Processando item com Id_Medicamento: {item.Id_Medicamento}, Quantidade: {item.Quantidade}");

                        if (medicamento_movimentacao.Tipo == 1)
                        {
                            await MedicamentoService.subtractMedicamentoEstoqueAsync(item.Id_Medicamento, item.Quantidade, _context);
                        }
                        else
                        {
                            await MedicamentoService.addMecitamentoEstoqueAsync(item.Id_Medicamento, item.Quantidade, _context);
                        }

                        Console.WriteLine($"Item processado com sucesso: Id_Medicamento: {item.Id_Medicamento}, Quantidade: {item.Quantidade}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Erro ao processar item com Id_Medicamento: {item.Id_Medicamento}, Quantidade: {item.Quantidade}. Erro: {ex.Message}");
                        throw;
                    }
                }

                _context.Medicamento_Movimentacao.Remove(medicamento_movimentacao);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Medicamento_Movimentacao com Id: {id} excluído com sucesso");
                return NoContent();
            }
            catch (Exception ex)
            {
                // Logar o erro detalhado para depuração
                Console.Error.WriteLine($"Erro ao deletar Medicamento_Movimentacao com Id: {id}. Erro: {ex.Message}");
                Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, "Ocorreu um erro ao processar a solicitação.");
            }
        }
    }
}
