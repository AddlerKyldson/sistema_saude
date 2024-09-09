namespace sistema_saude.Models
{
    public class InspecaoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string N_termo_inspecao { get; set; }
        public DateTimeOffset Data_inspecao { get; set; }
        public int Motivo_inspecao { get; set; }
        public int Id_estabelecimento { get; set; }
        public int Id_responsavel_tecnico { get; set; }
        public int Roteiro_Inspecao { get; set; }
        public string? Verificacao_Restaurantes { get; set; }
        public string? Verificacao_Supermercados { get; set; }
        public string? Verificacao_Escolas { get; set; }
        public string? Verificacao_Veiculos_Alimentos { get; set; }
        public int Status { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset Data_Cadastro { get; set; }
        public DateTimeOffset? Data_Alteracao { get; set; } // Pode ser NULL
        public string Slug { get; set; }

        public virtual EstabelecimentoDto? Estabelecimento { get; set; }

        public virtual UsuarioDto? Responsavel_Tecnico { get; set; }

    }
}
