public class Medicamento_MovimentacaoDto
{
    public string Descricao { get; set; }
    public DateTime Data { get; set; }
    public int Tipo { get; set; }
    public int Id_Usuario_Cadastro { get; set; }
    public List<Medicamento_Movimentacao_ItemDto> Medicamento_Movimentacao_Item { get; set; }
}
