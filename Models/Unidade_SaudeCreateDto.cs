public class Unidade_SaudeCreateDto
{
    public string Nome { get; set; }
    public string Cnes { get; set; }
    public int Tipo { get; set; }
    public string Logradouro { get; set; }
    public string Cep { get; set; }
    public string Numero { get; set; }
    public string Complemento { get; set; }
    public int Id_Bairro { get; set; }
    public int Status { get; set; }
    public int Id_Usuario_Cadastro { get; set; }
    public DateTimeOffset Data_Cadastro { get; set; }
    public string Slug { get; set; }
}
