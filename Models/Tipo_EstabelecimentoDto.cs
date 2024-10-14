public class Tipo_EstabelecimentoDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string? cnae { get; set; }
    public int? Grau_Risco { get; set; }
    public int? Passivo_Alvara_Sanitario { get; set; }
    public int? Passivo_Analise_Projeto { get; set; }
    public int Id_Serie { get; set; }
    public int Status { get; set; }
    public int Id_Usuario_Cadastro { get; set; }
    public DateTimeOffset Data_Cadastro { get; set; }
    public string Slug { get; set; }
}
