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
    public class MedicamentoController : ControllerBase
    {
        private readonly MyDbContext _context;

        public MedicamentoController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicamento>>> GetMedicamento()
        {
            return await _context.Medicamento.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento>> GetMedicamento(int id)
        {
            var medicamento = await _context.Medicamento.FindAsync(id);

            if (medicamento == null)
            {
                return NotFound();
            }

            return medicamento;
        }

        [HttpGet("CodigoBarras/{Codigo_Barras}")]
        public async Task<ActionResult<Medicamento>> GetMedicamentoByCodigoBarras(int Codigo_Barras)
        {
            var medicamento = await _context.Medicamento
                                    .FirstOrDefaultAsync(m => m.Codigo_Barras == Codigo_Barras);

            if (medicamento == null)
            {
                return NoContent();
            }

            return medicamento;
        }

        [HttpPost]
        public async Task<ActionResult<Medicamento>> PostMedicamento([FromBody] MedicamentoCreateDto medicamentoDto)
        {
            var medicamento = new Medicamento
            {
                Nome = medicamentoDto.Nome,
                Apelido = medicamentoDto.Apelido,
                Codigo_Barras = medicamentoDto.Codigo_Barras,
                Estoque = medicamentoDto.Estoque,
                Id_Usuario_Cadastro = medicamentoDto.Id_Usuario_Cadastro,
                Slug = medicamentoDto.Slug,
                Data_Cadastro = DateTime.UtcNow, // Definir a data de cadastro para agora
            };

            _context.Medicamento.Add(medicamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicamento", new { id = medicamento.Id }, medicamento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicamento(int id, [FromBody] MedicamentoDto medicamentoDto)
        {
            var medicamento = await _context.Medicamento.FindAsync(id);

            if (medicamento == null)
            {
                return NotFound();
            }

            medicamento.Nome = medicamentoDto.Nome;
            medicamento.Apelido = medicamentoDto.Apelido;
            medicamento.Codigo_Barras = medicamentoDto.Codigo_Barras;
            medicamento.Estoque = medicamentoDto.Estoque;
            medicamento.Data_Alteracao = DateTime.UtcNow;
            medicamento.Slug = medicamentoDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicamento(int id)
        {
            var medicamento = await _context.Medicamento.FindAsync(id);
            if (medicamento == null)
            {
                return NotFound();
            }

            _context.Medicamento.Remove(medicamento);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
