namespace sistema_saude.Models
{
    public class Cidade
    {
        public int Id { get; set; }
        public int Codigo_IBGE { get; set; }
        public string Nome { get; set; }
        public int Id_Regional { get; set; }
        public int Id_Estado { get; set; }
        public int Status { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL
        public string? Slug { get; set; }
        public ICollection<Bairro> Bairro { get; set; } // Lista de Bairros relacionados
        public ICollection<Estabelecimento> Estabelecimento { get; set; }
        public ICollection<Denuncia> Denuncia { get; set; }
        // Navegação para Regional
        public virtual Regional Regional { get; set; }

        //Navegação para Estado
        public virtual Estado Estado { get; set; }

        /* public virtual Bairro BairroNavigation { get; set; }
        public virtual Estado Id_Estado_CadastroNavigation { get; set; }
        public virtual Estado Id_Estado_AlteracaoNavigation { get; set; } */
    }
}
