namespace sistema_saude.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Email { get; set; }
        public string Senha { get; set; }
        public string? Telefone { get; set; }
        public string CPF { get; set; }
        public string CNS { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public int Bairro { get; set; }
        public string CEP { get; set; }
        public int Status { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL
        public string Permissoes { get; set; }
        public int? Escolaridade { get; set; }
        public int Tipo { get; set; }
        public string Slug { get; set; }

        public ICollection<Estabelecimento_Responsavel_Legal> Estabelecimento_Responsavel_Legal { get; set; }
        public ICollection<Inspecao> Inspecao { get; set; }

        /* public virtual Bairro BairroNavigation { get; set; }
        public virtual Usuario Id_Usuario_CadastroNavigation { get; set; }
        public virtual Usuario Id_Usuario_AlteracaoNavigation { get; set; } */
        
    }
}
