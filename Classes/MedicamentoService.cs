using Microsoft.EntityFrameworkCore;
using sistema_saude.Data;
using sistema_saude.Models;

public static class MedicamentoService
{
    public static async Task VerificarMedicamentoPorCodigoBarrasAsync(
        int codigoBarras,
        string nome,
        int quantidade,
        MyDbContext context
    )
    {
        // Criar um comando SQL para selecionar o medicamento com o código de barras especificado
        var sql = $"SELECT * FROM Medicamento WHERE Codigo_Barras = {codigoBarras}";

        // Executar o comando SQL
        var medicamento = await context.Medicamento.FromSqlRaw(sql).FirstOrDefaultAsync();

        if (medicamento != null)
        {
            // Medicamento encontrado, imprimir os detalhes
            Console.WriteLine(
                $"Medicamento encontrado: ID: {medicamento.Id}, Nome: {medicamento.Nome}, Código de Barras: {medicamento.Codigo_Barras}"
            );

            // Atualizar o estoque do medicamento
            medicamento.Estoque += quantidade;
            await context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Nenhum medicamento encontrado com o código de barras fornecido.");
            AdicionarNovoMedicamentoAsync(codigoBarras, nome, quantidade, context).Wait();
        }
    }

    private static async Task AdicionarNovoMedicamentoAsync(
        int codigoBarras,
        string nome,
        int quantidade,
        MyDbContext context
    )
    {
        var novoMedicamento = new Medicamento
        {
            Codigo_Barras = codigoBarras,
            Nome = nome,
            Apelido = nome,
            Slug = "apenas-teste",
            Estoque = quantidade,
            Data_Cadastro = DateTimeOffset.Now,
            // Adicione outras propriedades conforme necessário
        };

        context.Medicamento.Add(novoMedicamento);
        await context.SaveChangesAsync();

        Console.WriteLine("Novo medicamento adicionado ao banco de dados.");
    }

    public static async Task subtractMedicamentoEstoqueAsync(
        int codigoBarras,
        int quantidade,
        MyDbContext context
    )
    {
        // Criar um comando SQL para selecionar o medicamento com o código de barras especificado
        var sql = $"SELECT * FROM Medicamento WHERE Codigo_Barras = {codigoBarras}";

        // Executar o comando SQL
        var medicamento = await context.Medicamento.FromSqlRaw(sql).FirstOrDefaultAsync();

        if (medicamento != null)
        {
            // Medicamento encontrado, imprimir os detalhes
            Console.WriteLine(
                $"Medicamento encontrado: ID: {medicamento.Id}, Nome: {medicamento.Nome}, Código de Barras: {medicamento.Codigo_Barras}"
            );

            // Atualizar o estoque do medicamento
            medicamento.Estoque -= quantidade;
            await context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Nenhum medicamento encontrado com o código de barras fornecido.");
        }
    }
}
