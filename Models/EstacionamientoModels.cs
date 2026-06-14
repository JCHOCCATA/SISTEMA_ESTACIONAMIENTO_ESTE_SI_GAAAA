using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace SistemaEstacionamiento.Models;

public partial class EstacionamientoModels : DbContext
{
    public EstacionamientoModels()
    {
    }

    public EstacionamientoModels(DbContextOptions<EstacionamientoModels> options)
        : base(options)
    {
    }

    public virtual DbSet<Entidade> Entidades { get; set; }

    public virtual DbSet<PreciosEstacionamiento> PreciosEstacionamientos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SedeEstacionamiento> SedeEstacionamientos { get; set; }

    public virtual DbSet<Sitio> Sitios { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoVehiculo> TipoVehiculos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=acela.proxy.rlwy.net;port=28560;database=EstacionamientoSistema;user=root;password=hcMITeXwrPnzawGSzqFDkanxDEiXUFUB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.4.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Entidade>(entity =>
        {
            entity.HasKey(e => e.EntiId).HasName("PRIMARY");

            entity.HasIndex(e => e.TipoDocumentoFk, "fk_entidades_tipodocumento");

            entity.Property(e => e.EntiId).HasColumnName("entiId");
            entity.Property(e => e.EntiEstado)
                .HasMaxLength(20)
                .HasColumnName("entiEstado");
            entity.Property(e => e.EntiFechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("entiFechaCreacion");
            entity.Property(e => e.EntiFechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("entiFechaModificacion");
            entity.Property(e => e.EntiNroDocumento)
                .HasMaxLength(20)
                .HasColumnName("entiNroDocumento");
            entity.Property(e => e.EntiRazonSocial)
                .HasMaxLength(255)
                .HasColumnName("entiRazonSocial");
            entity.Property(e => e.EntiUsuarioCreacion)
                .HasMaxLength(50)
                .HasColumnName("entiUsuarioCreacion");
            entity.Property(e => e.EntiUsuarioModificacion)
                .HasMaxLength(50)
                .HasColumnName("entiUsuarioModificacion");
            entity.Property(e => e.TipoDocumentoFk).HasColumnName("TipoDocumentoFK");

            entity.HasOne(d => d.TipoDocumentoFkNavigation).WithMany(p => p.Entidades)
                .HasForeignKey(d => d.TipoDocumentoFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_entidades_tipodocumento");
        });

        modelBuilder.Entity<PreciosEstacionamiento>(entity =>
        {
            entity.HasKey(e => e.PreeId).HasName("PRIMARY");

            entity.ToTable("Precios_Estacionamiento");

            entity.HasIndex(e => e.SedeEstacionamientosFk, "fk_precios_sede");

            entity.HasIndex(e => e.TipoVehiculoFk, "fk_precios_tipovehiculo");

            entity.Property(e => e.PreeId).HasColumnName("preeId");
            entity.Property(e => e.PreeEstado)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("preeEstado");
            entity.Property(e => e.PreeFechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("preeFechaCreacion");
            entity.Property(e => e.PreeFechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("preeFechaModificacion");
            entity.Property(e => e.PreePrecio)
                .HasPrecision(10, 2)
                .HasColumnName("preePrecio");
            entity.Property(e => e.PreeUsuarioCreacion)
                .HasMaxLength(50)
                .HasColumnName("preeUsuarioCreacion");
            entity.Property(e => e.PreeUsuarioModificacion)
                .HasMaxLength(50)
                .HasColumnName("preeUsuarioModificacion");
            entity.Property(e => e.SedeEstacionamientosFk).HasColumnName("Sede_EstacionamientosFK");
            entity.Property(e => e.TipoVehiculoFk).HasColumnName("TipoVehiculoFK");

            entity.HasOne(d => d.SedeEstacionamientosFkNavigation).WithMany(p => p.PreciosEstacionamientos)
                .HasForeignKey(d => d.SedeEstacionamientosFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_precios_sede");

            entity.HasOne(d => d.TipoVehiculoFkNavigation).WithMany(p => p.PreciosEstacionamientos)
                .HasForeignKey(d => d.TipoVehiculoFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_precios_tipovehiculo");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleNombreRol)
                .HasMaxLength(50)
                .HasColumnName("roleNombreRol");
        });

        modelBuilder.Entity<SedeEstacionamiento>(entity =>
        {
            entity.HasKey(e => e.SeestId).HasName("PRIMARY");

            entity.ToTable("Sede_Estacionamientos");

            entity.Property(e => e.SeestId).HasColumnName("seestId");
            entity.Property(e => e.SeestEstado)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("seestEstado");
            entity.Property(e => e.SeestFechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("seestFechaCreacion");
            entity.Property(e => e.SeestFechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("seestFechaModificacion");
            entity.Property(e => e.SeestNombreEstacionamiento)
                .HasMaxLength(150)
                .HasColumnName("seestNombreEstacionamiento");
            entity.Property(e => e.SeestUbicacion)
                .HasMaxLength(255)
                .HasColumnName("seestUbicacion");
            entity.Property(e => e.SeestUsuarioCreacion)
                .HasMaxLength(50)
                .HasColumnName("seestUsuarioCreacion");
            entity.Property(e => e.SeestUsuarioModificacion)
                .HasMaxLength(50)
                .HasColumnName("seestUsuarioModificacion");
        });

        modelBuilder.Entity<Sitio>(entity =>
        {
            entity.HasKey(e => e.SitiId).HasName("PRIMARY");

            entity.HasIndex(e => e.SedeEstacionamientosFk, "fk_sitios_sede");

            entity.Property(e => e.SitiId).HasColumnName("sitiId");
            entity.Property(e => e.SedeEstacionamientosFk).HasColumnName("Sede_EstacionamientosFK");
            entity.Property(e => e.SitiEstado)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("sitiEstado");
            entity.Property(e => e.SitiFechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("sitiFechaCreacion");
            entity.Property(e => e.SitiFechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("sitiFechaModificacion");
            entity.Property(e => e.SitiNombreSitio)
                .HasMaxLength(50)
                .HasColumnName("sitiNombreSitio");
            entity.Property(e => e.SitiUsuarioCreacion)
                .HasMaxLength(50)
                .HasColumnName("sitiUsuarioCreacion");
            entity.Property(e => e.SitiUsuarioModificacion)
                .HasMaxLength(50)
                .HasColumnName("sitiUsuarioModificacion");

            entity.HasOne(d => d.SedeEstacionamientosFkNavigation).WithMany(p => p.Sitios)
                .HasForeignKey(d => d.SedeEstacionamientosFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sitios_sede");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.TidoId).HasName("PRIMARY");

            entity.ToTable("TipoDocumento");

            entity.Property(e => e.TidoId).HasColumnName("tidoId");
            entity.Property(e => e.TidoNombreDoc)
                .HasMaxLength(100)
                .HasColumnName("tidoNombreDoc");
        });

        modelBuilder.Entity<TipoVehiculo>(entity =>
        {
            entity.HasKey(e => e.TiveId).HasName("PRIMARY");

            entity.ToTable("TipoVehiculo");

            entity.Property(e => e.TiveId).HasColumnName("tiveId");
            entity.Property(e => e.TiveNombreVehiculo)
                .HasMaxLength(100)
                .HasColumnName("tiveNombreVehiculo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuaId).HasName("PRIMARY");

            entity.HasIndex(e => e.EntidadesFk, "fk_usuarios_entidades");

            entity.HasIndex(e => e.RolesFk, "fk_usuarios_roles");

            entity.HasIndex(e => e.SedeEstacionamientosFk, "fk_usuarios_sede");

            entity.Property(e => e.UsuaId).HasColumnName("usuaId");
            entity.Property(e => e.EntidadesFk).HasColumnName("EntidadesFK");
            entity.Property(e => e.RolesFk).HasColumnName("RolesFK");
            entity.Property(e => e.SedeEstacionamientosFk).HasColumnName("Sede_EstacionamientosFK");
            entity.Property(e => e.UsuaContrasenia)
                .HasMaxLength(255)
                .HasColumnName("usuaContrasenia");
            entity.Property(e => e.UsuaEstado)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("usuaEstado");
            entity.Property(e => e.UsuaFechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("usuaFechaCreacion");
            entity.Property(e => e.UsuaFechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("usuaFechaModificacion");
            entity.Property(e => e.UsuaNombre)
                .HasMaxLength(100)
                .HasColumnName("usuaNombre");
            entity.Property(e => e.UsuaUsuarioCreacion)
                .HasMaxLength(50)
                .HasColumnName("usuaUsuarioCreacion");
            entity.Property(e => e.UsuaUsuarioModificacion)
                .HasMaxLength(50)
                .HasColumnName("usuaUsuarioModificacion");

            entity.HasOne(d => d.EntidadesFkNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.EntidadesFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuarios_entidades");

            entity.HasOne(d => d.RolesFkNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolesFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuarios_roles");

            entity.HasOne(d => d.SedeEstacionamientosFkNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.SedeEstacionamientosFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuarios_sede");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
