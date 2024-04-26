namespace sistema_saude.Models
{
    public class Unidade_Saude
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnes { get; set; }
        public int Tipo { get; set; }
        public string Logradouro { get; set; }
        public int Id_Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public int Status { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL
        public string Slug { get; set; }

        // Navegação para Bairro
        public virtual Bairro Bairro { get; set; }

        /* public virtual Bairro BairroNavigation { get; set; }
        public virtual Estado Id_Estado_CadastroNavigation { get; set; }
        public virtual Estado Id_Estado_AlteracaoNavigation { get; set; } */
    }
}
