namespace sistema_saude.Models
{
    public class Denuncia
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTimeOffset? Data_Recebimento { get; set; }
        public string? Bairro { get; set; }
        public int? Id_Cidade { get; set; }
        public int Tipo_Denuncia { get; set; }
        public int Origem_Denuncia { get; set; }
        public int Forma_Recebimento { get; set; }
        public int? Id_Usuario { get; set; }
        public int? Escolaridade { get; set; }
        public string Texto_Denuncia { get; set; }
        public int Atendida { get; set; }
        public int? Orgao_Atendimento { get; set; }
        public DateTimeOffset? Data_Atendimento { get; set; }
        public int? Motivo_Nao_Atendimento { get; set; }
        public int Status { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL

        // Navegação para Estado
        public virtual Cidade Cidade { get; set; }

        /* public virtual Bairro BairroNavigation { get; set; }
        public virtual Estado Id_Estado_CadastroNavigation { get; set; }
        public virtual Estado Id_Estado_AlteracaoNavigation { get; set; } */
    }
}
