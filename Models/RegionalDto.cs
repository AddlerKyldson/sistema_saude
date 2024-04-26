public class RegionalDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Id_Estado { get; set; }
    public string SiglaEstado { get; set; }
    public int Status { get; set; }
    public int Id_Usuario_Cadastro { get; set; }
    public DateTimeOffset Data_Cadastro { get; set; }
    public string Slug { get; set; }
}
