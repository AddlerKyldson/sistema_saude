namespace sistema_saude.Models
{
    public class Estabelecimento
    {
        public int id { get; set; }
        public string razao_social { get; set; }
        public string nome_fantasia { get; set; }
        public string cnpj { get; set; }
        public string cnae { get; set; }
        public DateTimeOffset? data_inicio_funcionamento { get; set; }
        public int grau_risco { get; set; }
        public string inscricao_municipal { get; set; }
        public string inscricao_estadual { get; set; }
        public string logradouro { get; set; }
        public int id_bairro { get; set; }
        public string cep { get; set; }
        public string complemento { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public string protocolo_funcionamento { get; set; }
        public int passivo_alvara_sanitario { get; set; }
        public string n_alvara_sanitario { get; set; }
        public int coleta_residuos { get; set; }
        public int autuacao_visa { get; set; }
        public int forma_abastecimento { get; set; }
        public int status { get; set; }
        public int id_usuario_cadastro { get; set; }
        public int? id_usuario_alteracao { get; set; }
        public DateTimeOffset data_cadastro { get; set; }
        public DateTime? data_alteracao { get; set; }
        public string slug { get; set; }

         public virtual List<Estabelecimento_Responsavel_Legal> Estabelecimento_Responsavel_Legal_List { get; set; }

        public virtual Bairro Bairro { get; set; }
    }
}
