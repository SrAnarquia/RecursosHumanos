using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecursosHumanos.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Base> Bases { get; set; }

    public virtual DbSet<DatosReclutamiento> DatosReclutamientos { get; set; }

    public virtual DbSet<DatosReclutamientoReflejo> DatosReclutamientoReflejos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Fuente> Fuentes { get; set; }

    public virtual DbSet<Posicion> Posicions { get; set; }

    public virtual DbSet<Reclutador> Reclutadors { get; set; }

    public virtual DbSet<Sexo> Sexos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioEspejo> UsuarioEspejos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Base>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Base__3214EC074BC0B040");

            entity.ToTable("Base", "Reclutamiento");

            entity.Property(e => e.Base1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Base");
            entity.Property(e => e.FechaCreación)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<DatosReclutamiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Datos_Re__3214EC07A4DE686C");

            entity.ToTable("Datos_Reclutamiento", "Reclutamiento");

            entity.Property(e => e.Comentarios)
                .HasMaxLength(2555)
                .IsUnicode(false);
            entity.Property(e => e.FechaContacto)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaCreación)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdBaseNavigation).WithMany(p => p.DatosReclutamientos)
                .HasForeignKey(d => d.IdBase)
                .HasConstraintName("FK_Base_Nombre");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.DatosReclutamientos)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK_Empresa_Nombre");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.DatosReclutamientos)
                .HasForeignKey(d => d.IdEstatus)
                .HasConstraintName("FK_Estatus_Nombre");

            entity.HasOne(d => d.IdFuenteNavigation).WithMany(p => p.DatosReclutamientos)
                .HasForeignKey(d => d.IdFuente)
                .HasConstraintName("FK_Fuente_Nombre");

            entity.HasOne(d => d.IdPosicionNavigation).WithMany(p => p.DatosReclutamientos)
                .HasForeignKey(d => d.IdPosicion)
                .HasConstraintName("FK_Posicion_Nombre");

            entity.HasOne(d => d.IdReclutadorNavigation).WithMany(p => p.DatosReclutamientos)
                .HasForeignKey(d => d.IdReclutador)
                .HasConstraintName("FK_Reclutador_Nombre");

            entity.HasOne(d => d.IdSexoNavigation).WithMany(p => p.DatosReclutamientos)
                .HasForeignKey(d => d.IdSexo)
                .HasConstraintName("FK_Sexo_Nombre");
        });

        modelBuilder.Entity<DatosReclutamientoReflejo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Datos_Re__3214EC07155A07DC");

            entity.ToTable("Datos_Reclutamiento_Reflejo", "Reclutamiento");

            entity.Property(e => e.Base)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Comentarios)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Empresa)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Fuente)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Posicion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Reclutador)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Sexo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Empresa__3214EC0716581833");

            entity.ToTable("Empresa", "Reclutamiento");

            entity.Property(e => e.Empresa1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Empresa");
            entity.Property(e => e.FechaCreación)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Estatus__3214EC072F975721");

            entity.ToTable("Estatus", "Reclutamiento");

            entity.Property(e => e.Estatus1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Estatus");
            entity.Property(e => e.FechaCreación)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Fuente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Fuente__3214EC0784036524");

            entity.ToTable("Fuente", "Reclutamiento");

            entity.Property(e => e.FechaCreación)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Fuente1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Fuente");
        });

        modelBuilder.Entity<Posicion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Posicion__3214EC07E21C48C7");

            entity.ToTable("Posicion", "Reclutamiento");

            entity.Property(e => e.FechaCreación)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Posicion1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Posicion");
        });

        modelBuilder.Entity<Reclutador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reclutad__3214EC0739300128");

            entity.ToTable("Reclutador", "Reclutamiento");

            entity.Property(e => e.FechaCreación)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sexo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sexo__3214EC072F6FFB1B");

            entity.ToTable("Sexo", "Reclutamiento");

            entity.Property(e => e.FechaCreación)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Sexo1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Sexo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuarios__3214EC0764D14EAE");

            entity.ToTable("usuarios", "Usuario");

            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(255)
                .HasColumnName("correo");
            entity.Property(e => e.EsAdministrador).HasColumnName("es_administrador");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Usuario");
        });

        modelBuilder.Entity<UsuarioEspejo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UsuarioEspejo", "Usuario");

            entity.Property(e => e.Contraseña).HasMaxLength(255);
            entity.Property(e => e.Correo)
                .HasMaxLength(255)
                .HasColumnName("correo");
            entity.Property(e => e.EsAdministrador)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("es_administrador");
            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Usuario)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
