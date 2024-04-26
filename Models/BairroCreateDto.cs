public class BairroCreateDto
{
    public string Nome { get; set; }
    public int Id_Cidade { get; set; }
    public int Status { get; set; }
    public int Id_Usuario_Cadastro { get; set; }
    public DateTimeOffset Data_Cadastro { get; set; }
    public string Slug { get; set; }
}
