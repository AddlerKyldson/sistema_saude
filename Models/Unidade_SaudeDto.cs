public class Unidade_SaudeDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cnes { get; set; }
    public int Tipo { get; set; }
    public string Logradouro { get; set; }
    public int Id_Bairro { get; set; }
    public string Cep { get; set; }
    public string Numero { get; set; }
    public string Complemento { get; set; }
    public string? Nome_Cidade { get; set; }
    public string? Nome_Bairro { get; set; }
    public string? Sigla_Estado { get; set; }
    public int? Id_Estado { get; set; }
    public int Id_Cidade { get; set; }
    public int Status { get; set; }
    public int? Id_Usuario_Cadastro { get; set; }
    public DateTimeOffset Data_Cadastro { get; set; }
    public string Slug { get; set; }
}
