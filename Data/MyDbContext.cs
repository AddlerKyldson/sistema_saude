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

        // Outras tabelas conforme seus modelos

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Regional>()
                .HasOne(p => p.Estado) // Define o relacionamento
                .WithMany(b => b.Regional)
                .HasForeignKey(p => p.Id_Estado); // Chave estrangeira em Região

            // Configuração do relacionamento de Cidade com Regional
            modelBuilder.Entity<Cidade>()
                .HasOne(c => c.Regional)  // Cidade tem um Regional
                .WithMany(r => r.Cidade) // Regional tem muitas Cidades
                .HasForeignKey(c => c.Id_Regional);  // Chave estrangeira em Cidade

            // Configuração do relacionamento de Cidade com Estado
            modelBuilder.Entity<Cidade>()
                .HasOne(c => c.Estado)  // Cidade tem um Estado
                .WithMany(e => e.Cidade) // Estado tem muitas Cidades
                .HasForeignKey(c => c.Id_Estado);  // Chave estrangeira em Cidade

            // Configuração do relacionamento de Bairro com Cidade
            modelBuilder.Entity<Bairro>()
                .HasOne(b => b.Cidade)  // Bairro tem uma Cidade
                .WithMany(c => c.Bairro) // Cidade tem muitos Bairros
                .HasForeignKey(b => b.Id_Cidade);  // Chave estrangeira em Bairro

            // Configuração do relacionamento de Unidade_Saude com Bairro
            modelBuilder.Entity<Unidade_Saude>()
                .HasOne(u => u.Bairro)  // Unidade_Saude tem um Bairro
                .WithMany(b => b.Unidade_Saude) // Bairro tem muitas Unidades de Saúde
                .HasForeignKey(u => u.Id_Bairro);  // Chave estrangeira em Unidade_Saude
        }
    }
}
