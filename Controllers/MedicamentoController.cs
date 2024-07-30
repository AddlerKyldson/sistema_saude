using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_saude.Data;
using sistema_saude.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Medicamento>>> GetMedicamento([FromQuery] string? filtro_busca = null)
        {
            if (string.IsNullOrEmpty(filtro_busca))
            {
                return await _context.Medicamento.ToListAsync();
            }

            var medicamentosFiltrados = await _context.Medicamento
                .Where(m => m.Nome.Contains(filtro_busca) || m.Apelido.Contains(filtro_busca) || m.Codigo_Barras.ToString().Contains(filtro_busca))
                .ToListAsync();

            return medicamentosFiltrados;
        }

        [HttpGet("gerar_excel")]
        public async Task<IActionResult> GerarExcel()
        {
            var medicamentos = await _context.Medicamento.ToListAsync();

            if (medicamentos == null || medicamentos.Count == 0)
            {
                return NoContent();
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Medicamentos");

                // Cabeçalhos
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Nome";
                worksheet.Cells[1, 3].Value = "Descrição";
                worksheet.Cells[1, 4].Value = "Em Estoque";

                // Estilo dos cabeçalhos
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                // Preenchendo os dados
                for (int i = 0; i < medicamentos.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = medicamentos[i].Id;
                    worksheet.Cells[i + 2, 2].Value = medicamentos[i].Nome;
                    worksheet.Cells[i + 2, 3].Value = medicamentos[i].Apelido;
                    worksheet.Cells[i + 2, 4].Value = medicamentos[i].Estoque;
                }

                // Ajustando a largura das colunas
                worksheet.Cells.AutoFitColumns();

                // Gerando o arquivo Excel em memória
                var excelData = package.GetAsByteArray();

                // Retornando o arquivo Excel como download
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Medicamentos.xlsx");
            }
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

        [HttpGet("Form/{coluna}/{parametro}")]
        public async Task<ActionResult<List<Medicamento>>> GetMedicamentos(string coluna, string parametro)
        {
            // Exibir os valores no console
            System.Console.WriteLine($"Coluna: {coluna}");
            System.Console.WriteLine($"Parametro: {parametro}");

            // Verifica se a coluna informada é válida
            if (coluna != "nome" && coluna != "codigo_barras")
            {
                return BadRequest("Coluna inválida");
            }

            // Verificar se o tamanho de caracteres do parâmetro é igual ou maior a 3
            if (parametro.Length < 3)
            {
                return BadRequest("O parâmetro deve ter no mínimo 3 caracteres");
            }

            // Montar a consulta usando LINQ
            IQueryable<Medicamento> query = _context.Medicamento;

            if (coluna == "nome")
            {
                query = query.Where(m => m.Nome.Contains(parametro));
            }
            else if (coluna == "codigo_barras")
            {
                query = query.Where(m => m.Codigo_Barras.ToString().Contains(parametro));
            }

            var medicamentos = await query.ToListAsync();

            // Verificar se a lista de medicamentos está vazia
            if (medicamentos == null || medicamentos.Count == 0)
            {
                return NoContent();
            }

            return Ok(medicamentos);
        }



        [HttpGet("CodigoBarras/{Codigo_Barras}")]
        public async Task<ActionResult<Medicamento>> GetMedicamentoByCodigoBarras(string Codigo_Barras)
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
            try
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
            catch (Exception e)
            {
                //mostrar no log
                System.Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
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
