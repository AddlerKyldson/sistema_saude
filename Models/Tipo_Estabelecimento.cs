namespace sistema_saude.Models
{
    public class Tipo_Estabelecimento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Id_Serie { get; set; }
        public int Status { get; set; }
        public int Id_Usuario_Cadastro { get; set; }
        public int? Id_Usuario_Alteracao { get; set; }
        public DateTimeOffset Data_Cadastro { get; set; }
        public DateTimeOffset? Data_Alteracao { get; set; } // Pode ser NULL
        public string Slug { get; set; }


        public ICollection<Estabelecimento> Estabelecimento { get; set; }
        public virtual Serie Serie { get; set; }
    }
}