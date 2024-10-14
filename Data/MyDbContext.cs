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
        public DbSet<Estabelecimento_Responsavel_Tecnico> Estabelecimento_Responsavel_Tecnico { get; set; }
        public DbSet<Serie> Serie { get; set; }
        public DbSet<Tipo_Estabelecimento> Tipo_Estabelecimento { get; set; }
        public DbSet<Inspecao> Inspecao { get; set; }
        public DbSet<Denuncia> Denuncia { get; set; }


        // Outras tabelas conforme seus modelos

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Regional>()
                .HasOne(p => p.Estado) // Define o relacionamento
                .WithMany(b => b.Regional)
                .HasForeignKey(p => p.Id_Estado); // Chave estrangeira em Região

            modelBuilder
                .Entity<Cidade>()
                .HasOne(c => c.Regional) // Cidade tem um Regional opcionalmente
                .WithMany(r => r.Cidade) // Regional tem muitas Cidades
                .HasForeignKey(c => c.Id_Regional)
                .IsRequired(false); // Chave estrangeira opcional em Cidade

            modelBuilder
                .Entity<Cidade>()
                .HasOne(c => c.Estado) // Cidade tem um Estado opcionalmente
                .WithMany(e => e.Cidade) // Estado tem muitas Cidades
                .HasForeignKey(c => c.Id_Estado)
                .IsRequired(false); // Chave estrangeira opcional

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

            // Configuração do relacionamento de Estabelecimento com Cidade usando codigo_ibge
            modelBuilder
                .Entity<Estabelecimento>()
                .HasOne(e => e.Cidade) // Estabelecimento tem uma Cidade
                .WithMany(c => c.Estabelecimento) // Cidade tem muitos Estabelecimentos
                .HasForeignKey(e => e.id_cidade) // Chave estrangeira em Estabelecimento (codigo_ibge)
                .HasPrincipalKey(c => c.Codigo_IBGE); // Define codigo_ibge como chave principal em Cidade


            //configurar relação entre estabelecimento_responsavel_legal e usuario
            modelBuilder.Entity<Estabelecimento_Responsavel_Legal>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Estabelecimento_Responsavel_Legal)
                .HasForeignKey(e => e.Id_Usuario);

            //configurar relação entre estabelecimento_responsavel_tecnico e usuario
            modelBuilder.Entity<Estabelecimento_Responsavel_Tecnico>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Estabelecimento_Responsavel_Tecnico)
                .HasForeignKey(e => e.Id_Usuario);


            modelBuilder.Entity<Estabelecimento>()
            .HasMany(e => e.Estabelecimento_Responsavel_Legal_List)
            .WithOne(rl => rl.Estabelecimento)
            .HasForeignKey(rl => rl.Id_Estabelecimento) // Nome da coluna da chave estrangeira
            .HasConstraintName("FK_Estabelecimento_Responsavel_Legal_Estabelecimento");

            modelBuilder.Entity<Estabelecimento>()
            .HasMany(e => e.Estabelecimento_Responsavel_Tecnico_List)
            .WithOne(rt => rt.Estabelecimento)
            .HasForeignKey(rt => rt.Id_Estabelecimento) // Nome da coluna da chave estrangeira
            .HasConstraintName("FK_Estabelecimento_Responsavel_Tecnico_Estabelecimento");

            // Configuração do relacionamento de Série com TipoEstabelecimento
            modelBuilder
                .Entity<Serie>()
                .HasMany(s => s.Tipo_Estabelecimento) // Série tem muitos Tipos de Estabelecimento
                .WithOne(t => t.Serie) // Tipo de Estabelecimento tem uma Série
                .HasForeignKey(t => t.Id_Serie); // Chave estrangeira em TipoEstabelecimento

            // Configuração do relacionamento de TipoEstabelecimento com Estabelecimento    
            modelBuilder
                .Entity<Tipo_Estabelecimento>()
                .HasMany(te => te.Estabelecimento) // Tipo de Estabelecimento tem muitos Estabelecimentos
                .WithOne(e => e.Tipo_Estabelecimento) // Estabelecimento tem um Tipo de Estabelecimento
                .HasForeignKey(e => e.id_tipo_estabelecimento); // Chave estrangeira em Estabelecimento

            // Configuração do relacionamento de Inspecao com Estabelecimento
            modelBuilder
                .Entity<Inspecao>()
                .HasOne(i => i.Estabelecimento) // Inspecao tem um Estabelecimento
                .WithMany(e => e.Inspecao) // Estabelecimento tem muitas Inspeções
                .HasForeignKey(i => i.Id_estabelecimento); // Chave estrangeira em Inspecao

            // Configuração do relacionamento de Inspecao com Usuario
            modelBuilder
                .Entity<Inspecao>()
                .HasOne(i => i.Usuario) // Inspecao tem um Usuario
                .WithMany(u => u.Inspecao) // Usuario tem muitas Inspeções
                .HasForeignKey(i => i.Id_Usuario_Cadastro); // Chave estrangeira em Inspecao

            // Configuração do relacionamento de Inspecao Responsavel Tecnico com Usuario
            modelBuilder
                .Entity<Inspecao>()
                .HasOne(i => i.Usuario) // Inspecao tem um Usuario
                .WithMany(u => u.Inspecao) // Usuario tem muitas Inspeções
                .HasForeignKey(i => i.Id_responsavel_tecnico); // Chave estrangeira em Inspecao

            // Configuração do relacionamento de Denuncia com Bairro
            modelBuilder
                .Entity<Denuncia>()
                .HasOne(d => d.Cidade) // Denuncia tem um Bairro
                .WithMany(b => b.Denuncia) // Bairro tem muitas Denúncias
                .HasForeignKey(d => d.Id_Cidade); // Chave estrangeira em Denuncia
        }
    }
}
