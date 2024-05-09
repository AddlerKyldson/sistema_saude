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

        // GET: api/Medicamento_Movimentacaos
        [HttpGet]
        public async Task<
            ActionResult<IEnumerable<Medicamento_Movimentacao>>
        > GetMedicamento_Movimentacao()
        {
            return await _context.Medicamento_Movimentacao.ToListAsync();
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
                        
                        await MedicamentoService.VerificarMedicamentoPorCodigoBarrasAsync(itemDto.Codigo_Barras, itemDto.Nome, itemDto.Quantidade, _context);

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
        
            //subtraimos a quantidade de medicamentos do estoque

            var medicamento_movimentacao_item = await _context.Medicamento_Movimentacao_Item.Where(x => x.Id_Medicamento_Movimentacao == id).ToListAsync();

            foreach (var item in medicamento_movimentacao_item)
            {
                //mostrar item no console
                Console.WriteLine(item.Id_Medicamento);
                await MedicamentoService.subtractMedicamentoEstoqueAsync(item.Id_Medicamento, item.Quantidade, _context);
            }

            var medicamento_movimentacao = await _context.Medicamento_Movimentacao.FindAsync(id);
            if (medicamento_movimentacao == null)
            {
                return NotFound();
            }

            _context.Medicamento_Movimentacao.Remove(medicamento_movimentacao);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
