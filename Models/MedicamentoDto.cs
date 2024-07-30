public class MedicamentoDto
{
    public string Nome { get; set; }
    public string Apelido { get; set; }
    public string Codigo_Barras { get; set; }
    public int Estoque { get; set; }
    public int Id_Usuario_Cadastro { get; set; }
    public DateTimeOffset Data_Cadastro { get; set; }
    public string Slug { get; set; }
}
