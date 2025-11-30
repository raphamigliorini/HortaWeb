using Esg.Horta.Entities;
using FiapWebAluno.Models;
using Microsoft.EntityFrameworkCore;

namespace FiapWebAluno.Data.Contexts
{
    /// <summary>
    /// DbContext principal da aplicação da horta ESG.
    /// Centraliza o mapeamento das entidades para as tabelas Oracle.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        // Construtor protegido usado internamente pelo EF (por exemplo, em testes).
        protected DatabaseContext()
        {
        }

        // ==========================
        // DbSets (tabelas)
        // ==========================

        public DbSet<Especie> Especies { get; set; } = null!;
        public DbSet<Canteiro> Canteiros { get; set; } = null!;
        public DbSet<SensorUmidade> SensoresUmidade { get; set; } = null!;
        public DbSet<Irrigacao> Irrigacoes { get; set; } = null!;
        public DbSet<Doacao> Doacoes { get; set; } = null!;
        public DbSet<Colheita> Colheitas { get; set; } = null!;

        /// <summary>
        /// Configuração de mapeamento das entidades para o Oracle.
        /// Aqui deixamos explícito o tipo das colunas, tamanhos, PKs e relacionamentos.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ==========================
            // ESPECIE
            // ==========================
            modelBuilder.Entity<Especie>(entity =>
            {
                entity.ToTable("ESPECIE");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("ID")
                      .ValueGeneratedNever();

                // Nome pode ser NULL no Oracle, então IsRequired(false) e string?
                entity.Property(e => e.Nome)
                      .HasColumnName("NOME")
                      .HasColumnType("VARCHAR2(60)")
                      .HasMaxLength(60)
                      .IsRequired(false);
            });

            // ==========================
            // CANTEIRO
            // ==========================
            modelBuilder.Entity<Canteiro>(entity =>
            {
                entity.ToTable("CANTEIRO");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id)
                      .HasColumnName("ID")
                      .ValueGeneratedNever();

                entity.Property(c => c.Nome)
                      .HasColumnName("NOME")
                      .HasColumnType("VARCHAR2(255)")
                      .HasMaxLength(255)
                      .IsRequired(false); // aceita NULL

                entity.Property(c => c.EspecieId)
                      .HasColumnName("ESPECIE_ID");

                entity.Property(c => c.AreaM2)
                      .HasColumnName("AREA_M2")
                      .HasColumnType("NUMBER(7,2)");

                entity.Property(c => c.MetaDoacaoKg)
                      .HasColumnName("META_DOACAO_KG")
                      .HasColumnType("NUMBER(9,2)");
            });

            // ==========================
            // SENSOR_UMIDADE
            // ==========================
            modelBuilder.Entity<SensorUmidade>(entity =>
            {
                entity.ToTable("TB_SENSOR_UMIDADE");

                entity.HasKey(s => s.IdSensor);

                entity.Property(s => s.IdSensor)
                      .HasColumnName("ID_SENSOR")
                      .ValueGeneratedNever();

                entity.Property(s => s.IdCanteiro)
                      .HasColumnName("ID_CANTEIRO");

                entity.Property(s => s.DataHora)
                      .HasColumnName("DATA_HORA")
                      .HasColumnType("DATE");

                entity.Property(s => s.PercentualUmidade)
                      .HasColumnName("PERCENTUAL_UMIDADE")
                      .HasColumnType("NUMBER(5,2)");

                entity.HasOne(s => s.Canteiro)
                      .WithMany(tc => tc.SensoresUmidade)
                      .HasForeignKey(s => s.IdCanteiro);
            });

            // ==========================
            // IRRIGACAO
            // ==========================
            modelBuilder.Entity<Irrigacao>(entity =>
            {
                entity.ToTable("TB_IRRIGACAO");

                entity.HasKey(i => i.IdIrrigacao);

                entity.Property(i => i.IdIrrigacao)
                      .HasColumnName("ID_IRRIGACAO")
                      .ValueGeneratedNever();

                entity.Property(i => i.IdCanteiro)
                      .HasColumnName("ID_CANTEIRO");

                entity.Property(i => i.DataHora)
                      .HasColumnName("DATA_HORA")
                      .HasColumnType("DATE");

                entity.Property(i => i.Litros)
                      .HasColumnName("LITROS")
                      .HasColumnType("NUMBER(9,2)");

                entity.Property(i => i.Origem)
                      .HasColumnName("ORIGEM")
                      .HasColumnType("VARCHAR2(20)")
                      .HasMaxLength(20);

                entity.Property(i => i.Observacao)
                      .HasColumnName("OBSERVACAO")
                      .HasColumnType("VARCHAR2(200)")
                      .HasMaxLength(200);

                entity.HasOne(i => i.Canteiro)
                      .WithMany(tc => tc.Irrigacoes)
                      .HasForeignKey(i => i.IdCanteiro);
            });

            // ==========================
            // DOACAO
            // ==========================
            modelBuilder.Entity<Doacao>(entity =>
            {
                entity.ToTable("TB_DOACAO");

                entity.HasKey(d => d.IdDoacao);

                entity.Property(d => d.IdDoacao)
                      .HasColumnName("ID_DOACAO")
                      .ValueGeneratedNever();

                entity.Property(d => d.IdColheita)
                      .HasColumnName("ID_COLHEITA");

                entity.Property(d => d.Entidade)
                      .HasColumnName("ENTIDADE")
                      .HasColumnType("VARCHAR2(120)")
                      .HasMaxLength(120);

                entity.Property(d => d.QuantidadeKg)
                      .HasColumnName("QUANTIDADE_KG")
                      .HasColumnType("NUMBER(9,2)");

                entity.Property(d => d.DataHora)
                      .HasColumnName("DATA_HORA")
                      .HasColumnType("DATE");

                entity.HasOne(d => d.Colheita)
                      .WithMany(c => c.Doacoes)
                      .HasForeignKey(d => d.IdColheita);
            });

            // ==========================
            // COLHEITA
            // ==========================
            modelBuilder.Entity<Colheita>(entity =>
            {
                entity.ToTable("TB_COLHEITA");

                entity.HasKey(c => c.IdColheita);

                entity.Property(c => c.IdColheita)
                      .HasColumnName("ID_COLHEITA")
                      .ValueGeneratedNever();

                entity.Property(c => c.IdCanteiro)
                      .HasColumnName("ID_CANTEIRO");

                entity.Property(c => c.DataHora)
                      .HasColumnName("DATA_HORA")
                      .HasColumnType("DATE");

                entity.Property(c => c.QuantidadeKg)
                      .HasColumnName("QUANTIDADE_KG")
                      .HasColumnType("NUMBER(9,2)");

                entity.Property(c => c.Destino)
                      .HasColumnName("DESTINO")
                      .HasColumnType("VARCHAR2(20)")
                      .HasMaxLength(20);

                entity.Property(c => c.DestinoSugerido)
                      .HasColumnName("DESTINO_SUGERIDO")
                      .HasColumnType("VARCHAR2(20)")
                      .HasMaxLength(20);

                entity.HasOne(c => c.Canteiro)
                      .WithMany(tc => tc.Colheitas)
                      .HasForeignKey(c => c.IdCanteiro);

                entity.HasMany(c => c.Doacoes)
                      .WithOne(d => d.Colheita)
                      .HasForeignKey(d => d.IdColheita);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
