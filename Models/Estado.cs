namespace sistema_saude.Models
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public int? Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset? Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL
        public string? Slug { get; set; }

        // Relacionamento com Região
        
        public virtual ICollection<Regional> Regional { get; set; }

        // Relacionamento com Cidade
        public virtual ICollection<Cidade> Cidade { get; set; }

        /* public virtual Bairro BairroNavigation { get; set; }
        public virtual Estado Id_Estado_CadastroNavigation { get; set; }
        public virtual Estado Id_Estado_AlteracaoNavigation { get; set; } */
    }
}
