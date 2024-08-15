namespace sistema_saude.Models
{
    public class Estabelecimento_Responsavel_Legal
    {
        public int? Id { get; set; }
        public int Id_Estabelecimento { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset? Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL

        //Navegação para Medicamento_Movimentacao
        public virtual Estabelecimento Estabelecimento { get; set; }

        public virtual Usuario Usuario { get; set; }

        /* public virtual Bairro BairroNavigation { get; set; }
        public virtual Estado Id_Estado_CadastroNavigation { get; set; }
        public virtual Estado Id_Estado_AlteracaoNavigation { get; set; } */
    }
}
