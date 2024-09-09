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
    public class UsuarioController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UsuarioController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario([FromQuery] string? filtro_busca = null, [FromQuery] int? tipo = null, [FromQuery] int? page = null, [FromQuery] int? perPage = null)
        {
            // Consulta básica de usuários
            var query = _context.Usuario.AsQueryable();

            // Filtragem por busca, se aplicável
            if (!string.IsNullOrEmpty(filtro_busca))
            {
                query = query.Where(u => u.Nome.Contains(filtro_busca) || u.Email.Contains(filtro_busca) || u.CPF.Contains(filtro_busca) || u.CNS.Contains(filtro_busca));
            }

            // Filtragem por tipo, se aplicável
            if (tipo != null)
            {
                query = query.Where(u => u.Tipo == tipo);
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
                var usuariosFiltrados = await query
                    .OrderBy(u => u.Nome) // Adicione uma ordenação padrão ou baseada em parâmetros
                    .Skip((page.Value - 1) * perPage.Value)
                    .Take(perPage.Value)
                    .ToListAsync();

                // Retorna os dados paginados e o total de itens
                return Ok(new
                {
                    total,
                    dados = usuariosFiltrados
                });
            }
            else
            {
                // Se page ou perPage forem nulos, lista todos os usuários
                var todosUsuarios = await query.ToListAsync();
                return Ok(new
                {
                    total = todosUsuarios.Count,
                    dados = todosUsuarios
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                Senha = usuarioDto.Senha,
                Telefone = usuarioDto.Telefone,
                CPF = usuarioDto.CPF,
                CNS = usuarioDto.CNS,
                Logradouro = usuarioDto.Logradouro,
                Numero = usuarioDto.Numero,
                Complemento = usuarioDto.Complemento,
                Bairro = usuarioDto.Bairro,
                CEP = usuarioDto.CEP,
                Status = usuarioDto.Status,
                Id_Usuario_Cadastro = usuarioDto.Id_Usuario_Cadastro,
                Slug = usuarioDto.Slug,
                Permissoes = usuarioDto.Permissoes,
                Tipo = usuarioDto.Tipo,
                Data_Cadastro = DateTime.UtcNow, // Definir a data de cadastro para agora
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioDto usuarioDto)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Nome = usuarioDto.Nome;
            usuario.Email = usuarioDto.Email;
            usuario.Senha = usuarioDto.Senha;
            usuario.Telefone = usuarioDto.Telefone;
            usuario.CPF = usuarioDto.CPF;
            usuario.CNS = usuarioDto.CNS;
            usuario.Logradouro = usuarioDto.Logradouro;
            usuario.Numero = usuarioDto.Numero;
            usuario.Complemento = usuarioDto.Complemento;
            usuario.Bairro = usuarioDto.Bairro;
            usuario.CEP = usuarioDto.CEP;
            usuario.Status = usuarioDto.Status;
            usuario.Id_Usuario_Cadastro = usuarioDto.Id_Usuario_Cadastro;
            usuario.Permissoes = usuarioDto.Permissoes;
            usuario.Tipo = usuarioDto.Tipo;
            usuario.Slug = usuarioDto.Slug;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna uma resposta 204 No Content para indicar sucesso sem conteúdo de retorno
        }
    }
}
