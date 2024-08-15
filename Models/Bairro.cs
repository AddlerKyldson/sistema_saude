namespace sistema_saude.Models
{
    public class Bairro
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Id_Cidade { get; set; }
        public int Status { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL
        public string Slug { get; set; }
        public ICollection<Unidade_Saude> Unidade_Saude { get; set; } // Lista de Unidades relacionadas
        public ICollection<Estabelecimento> Estabelecimento { get; set; }

        // Navegação para Regional
        public virtual Cidade Cidade { get; set; }

        /* public virtual Bairro BairroNavigation { get; set; }
        public virtual Estado Id_Estado_CadastroNavigation { get; set; }
        public virtual Estado Id_Estado_AlteracaoNavigation { get; set; } */
    }
}
