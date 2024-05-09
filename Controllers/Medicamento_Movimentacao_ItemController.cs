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
    public class Medicamento_Movimentacao_ItemController : ControllerBase
    {
        private readonly MyDbContext _context;

        public Medicamento_Movimentacao_ItemController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicamento_Movimentacao_Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicamento_Movimentacao_Item>>> GetMedicamento_Movimentacao_Item()
        {
            return await _context.Medicamento_Movimentacao_Item.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento_Movimentacao_Item>> GetMedicamento_Movimentacao_Item(int id)
        {
            var medicamento_movimentacao_item = await _context.Medicamento_Movimentacao_Item.FindAsync(id);

            if (medicamento_movimentacao_item == null)
            {
                return NotFound();
            }

            return medicamento_movimentacao_item;
        }

        [HttpGet("Movimentacao/{id_movimentacao}")]
        public async Task<ActionResult<IEnumerable<Medicamento_Movimentacao_Item>>> GetMedicamento_Movimentacao_ItemByMovimentacao(int id_movimentacao)
        {
            var medicamento_movimentacao_item = await _context.Medicamento_Movimentacao_Item
                                    .Include(med => med.Medicamento)
                                    .Where(m => m.Id_Medicamento_Movimentacao == id_movimentacao)
                                    .ToListAsync();

            if (medicamento_movimentacao_item.Count == 0)
            {
                return medicamento_movimentacao_item;
            }

            return medicamento_movimentacao_item;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicamento_Movimentacao_Item(int id)
        {
            var medicamento_movimentacao_item = await _context.Medicamento_Movimentacao_Item.FindAsync(id);
            if (medicamento_movimentacao_item == null)
            {
                return NotFound();
            }

            _context.Medicamento_Movimentacao_Item.Remove(medicamento_movimentacao_item);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
