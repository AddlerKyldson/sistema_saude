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
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.Usuario.ToListAsync();
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
