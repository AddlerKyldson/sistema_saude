using Microsoft.EntityFrameworkCore;
using sistema_saude.Data;
using sistema_saude.Models;

public static class UsuarioService
{
    public static async Task<int> VerificarUsuarioPorCPFAsync(
    Estabelecimento_Responsavel_LegalDto dados,
    MyDbContext context)
    {
        var usuario = await context.Usuario
            .Where(u => u.CPF == dados.CPF)
            .Select(u => u.Id)
            .FirstOrDefaultAsync();

        // Retorna o ID do usuário ou 0 se não for encontrado

        if (usuario == 0)
        {

            //mostrar log dizendo que cpf não foi encontrado
            Console.WriteLine("CPF não encontrado no banco de dados.");

            usuario = await AdicionarNovoUsuarioAsync(
                dados,
                context
            );
        }
        else
        {
            Console.WriteLine("CPF encontrado no banco de dados.");
        }

        return usuario;
    }

    private static async Task<int> AdicionarNovoUsuarioAsync(
        Estabelecimento_Responsavel_LegalDto dados,
        MyDbContext context
    )
    {
        var novoUsuario = new Usuario
        {
            CPF = dados.CPF,
            Nome = dados.nome_responsavel,
            Data_Cadastro = DateTimeOffset.Now,
            Escolaridade = dados.escolaridade,
            Email = dados.Email,
            Senha = dados.CPF,
            Status = 1,
            Id_Usuario_Cadastro = 1,
            Slug = dados.CPF,
            Tipo = 9,
            Telefone = "",
            CNS = "",
            Logradouro = "",
            Numero = "",
            Complemento = "",
            Bairro = '0',
            CEP = "",
            Permissoes = ""
        };

        context.Usuario.Add(novoUsuario);
        await context.SaveChangesAsync();

        Console.WriteLine("Novo usuário adicionado ao banco de dados.");

        //retorne o id do novo usuário
        return novoUsuario.Id;
    }
}
