public class BairroDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string? Nome_Cidade { get; set; }
    public string? Sigla_Estado { get; set; }
    public int? Id_Estado { get; set; }
    public int Id_Cidade { get; set; }
    public int Status { get; set; }
    public int? Id_Usuario_Cadastro { get; set; }
    public DateTimeOffset Data_Cadastro { get; set; }
    public string Slug { get; set; }

    
}
