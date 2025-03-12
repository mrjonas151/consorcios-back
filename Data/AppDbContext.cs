using ConsorcioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsorcioAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Administradora> Administradoras { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Cota> Cotas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grupo>()
            .HasOne(g => g.Administradora)
            .WithMany(a => a.Grupos)
            .HasForeignKey(g => g.AdministradoraId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cota>()
            .HasOne(c => c.Grupo)
            .WithMany(g => g.Cotas)
            .HasForeignKey(c => c.GrupoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cota>()
            .HasOne(c => c.Cliente)
            .WithMany(cl => cl.Cotas)
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Administradora>()
            .HasIndex(a => a.CNPJ)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.CPF)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Cota>()
            .HasIndex(c => new { c.GrupoId, c.NumeroCota })
            .IsUnique();
    }
}