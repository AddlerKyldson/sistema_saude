namespace sistema_saude.Models
{
    public class Medicamento_Movimentacao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTimeOffset Data { get; set; }
        public int Tipo { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL

        // Relacionamento com Medicamento_Movimentacao_Item
        public virtual ICollection<Medicamento_Movimentacao_Item> Medicamento_Movimentacao_Item { get; set; }
        
        /* public virtual Bairro BairroNavigation { get; set; }
        public virtual Estado Id_Estado_CadastroNavigation { get; set; }
        public virtual Estado Id_Estado_AlteracaoNavigation { get; set; } */
    }
}
