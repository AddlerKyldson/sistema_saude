public class EstadoDto
{
    public string Nome { get; set; }
    public string Sigla { get; set; }
    public int? Id_Usuario_Cadastro { get; set; }
    public DateTimeOffset? Data_Cadastro { get; set; }
    public string? Slug { get; set; }
}
