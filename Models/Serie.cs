namespace sistema_saude.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset Data_Cadastro { get; set; }
        public DateTime? Data_Alteracao { get; set; } // Pode ser NULL
        public string Slug { get; set; }

        // List Estabelecimento
        public virtual ICollection<Estabelecimento> Estabelecimento { get; set; }
    }
}
