namespace sistema_saude.Models
{
    public class Medicamento_Movimentacao_Item
    {
        public int Id { get; set; }
        public string Id_Medicamento { get; set; }
        public int Id_Medicamento_Movimentacao { get; set; }
        public int Quantidade { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset? Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL

        //Navegação para Medicamento_Movimentacao
        public virtual Medicamento_Movimentacao Medicamento_Movimentacao { get; set; }

        public virtual Medicamento Medicamento { get; set; }

        /* public virtual Bairro BairroNavigation { get; set; }
        public virtual Estado Id_Estado_CadastroNavigation { get; set; }
        public virtual Estado Id_Estado_AlteracaoNavigation { get; set; } */
    }
}
