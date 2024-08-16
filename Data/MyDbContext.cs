using Microsoft.EntityFrameworkCore;
using sistema_saude.Models;

namespace sistema_saude.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Regional> Regional { get; set; }
        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Bairro> Bairro { get; set; }
        public DbSet<Unidade_Saude> Unidade_Saude { get; set; }
        public DbSet<Medicamento> Medicamento { get; set; }
        public DbSet<Medicamento_Movimentacao> Medicamento_Movimentacao { get; set; }
        public DbSet<Medicamento_Movimentacao_Item> Medicamento_Movimentacao_Item { get; set; }
        public DbSet<Estabelecimento> Estabelecimento { get; set; }
        public DbSet<Estabelecimento_Responsavel_Legal> Estabelecimento_Responsavel_Legal { get; set; }
        public DbSet<Serie> Serie { get; set; }

        // Outras tabelas conforme seus modelos

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Regional>()
                .HasOne(p => p.Estado) // Define o relacionamento
                .WithMany(b => b.Regional)
                .HasForeignKey(p => p.Id_Estado); // Chave estrangeira em Região

            // Configuração do relacionamento de Cidade com Regional
            modelBuilder
                .Entity<Cidade>()
                .HasOne(c => c.Regional) // Cidade tem um Regional
                .WithMany(r => r.Cidade) // Regional tem muitas Cidades
                .HasForeignKey(c => c.Id_Regional); // Chave estrangeira em Cidade

            // Configuração do relacionamento de Cidade com Estado
            modelBuilder
                .Entity<Cidade>()
                .HasOne(c => c.Estado) // Cidade tem um Estado
                .WithMany(e => e.Cidade) // Estado tem muitas Cidades
                .HasForeignKey(c => c.Id_Estado); // Chave estrangeira em Cidade

            // Configuração do relacionamento de Bairro com Cidade
            modelBuilder
                .Entity<Bairro>()
                .HasOne(b => b.Cidade) // Bairro tem uma Cidade
                .WithMany(c => c.Bairro) // Cidade tem muitos Bairros
                .HasForeignKey(b => b.Id_Cidade); // Chave estrangeira em Bairro

            // Configuração do relacionamento de Unidade_Saude com Bairro
            modelBuilder
                .Entity<Unidade_Saude>()
                .HasOne(u => u.Bairro) // Unidade_Saude tem um Bairro
                .WithMany(b => b.Unidade_Saude) // Bairro tem muitas Unidades de Saúde
                .HasForeignKey(u => u.Id_Bairro); // Chave estrangeira em Unidade_Saude

            modelBuilder
                .Entity<Medicamento_Movimentacao_Item>()
                .HasOne(m => m.Medicamento_Movimentacao)
                .WithMany(m => m.Medicamento_Movimentacao_Item)
                .HasForeignKey(m => m.Id_Medicamento_Movimentacao);

            modelBuilder.Entity<Medicamento_Movimentacao_Item>()
                .HasOne(m => m.Medicamento)
                .WithMany(m => m.Medicamento_Movimentacao_Item)
                .HasForeignKey(m => m.Id_Medicamento) // Chave estrangeira em Medicamento_Movimentacao_Item
                .HasPrincipalKey(m => m.Codigo_Barras); // Chave primária em Medicamento

            // Configuração do relacionamento de Estabelecimento com Bairro
            modelBuilder
            .Entity<Estabelecimento>()
            .HasOne(e => e.Bairro) // Defina a propriedade de navegação correta
            .WithMany(b => b.Estabelecimento) // Relacionamento inverso, se aplicável
            .HasForeignKey(e => e.id_bairro) // Especifica a chave estrangeira correta
            .OnDelete(DeleteBehavior.Restrict); // Ajuste o comportamento de deleção se necessário

            //configurar relação entre estabelecimento_responsavel_legal e usuario
            modelBuilder.Entity<Estabelecimento_Responsavel_Legal>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Estabelecimento_Responsavel_Legal)
                .HasForeignKey(e => e.Id_Usuario);


            modelBuilder.Entity<Estabelecimento>()
            .HasMany(e => e.Estabelecimento_Responsavel_Legal_List)
            .WithOne(rl => rl.Estabelecimento)
            .HasForeignKey(rl => rl.Id_Estabelecimento) // Nome da coluna da chave estrangeira
            .HasConstraintName("FK_Estabelecimento_Responsavel_Legal_Estabelecimento");


            // Configuração do relacionamento de Serie com Estabelecimento
            

        }
    }
}
