using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace oracleDataAcess.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Dlleventum> Dlleventa { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=C##bantrax; Password=123456;Data Source=localhost:1521/xe;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##BANTRAX")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("SYS_C008321");

            entity.ToTable("CLIENTES");

            entity.HasIndex(e => e.Ci, "SYS_C008322").IsUnique();

            entity.Property(e => e.IdClient)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_CLIENT");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("APELLIDO");
            entity.Property(e => e.Ci)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CI");
            entity.Property(e => e.Edad)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("EDAD");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Dlleventum>(entity =>
        {
            entity.HasKey(e => e.Iddventa).HasName("SYS_C008329");

            entity.ToTable("DLLEVENTA");

            entity.Property(e => e.Iddventa)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("IDDVENTA");
            entity.Property(e => e.Cantidad)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CANTIDAD");
            entity.Property(e => e.Idprod)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("IDPROD");
            entity.Property(e => e.Idvta)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("IDVTA");
            entity.Property(e => e.Subtotal)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SUBTOTAL");

            entity.HasOne(d => d.IdprodNavigation).WithMany(p => p.Dlleventa)
                .HasForeignKey(d => d.Idprod)
                .HasConstraintName("FKPROD");

            entity.HasOne(d => d.IdvtaNavigation).WithMany(p => p.Dlleventa)
                .HasForeignKey(d => d.Idvta)
                .HasConstraintName("FKVTA");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Idproducto).HasName("SYS_C008324");

            entity.ToTable("PRODUCTOS");

            entity.Property(e => e.Idproducto)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("IDPRODUCTO");
            entity.Property(e => e.Cantidad)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CANTIDAD");
            entity.Property(e => e.Nombreprod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBREPROD");
            entity.Property(e => e.Preciounit)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PRECIOUNIT");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.Idventa).HasName("SYS_C008326");

            entity.ToTable("VENTA");

            entity.Property(e => e.Idventa)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("IDVENTA");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.Fchaope)
                .HasColumnType("DATE")
                .HasColumnName("FCHAOPE");
            entity.Property(e => e.Idclient)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("IDCLIENT");
            entity.Property(e => e.Montototal)
                .HasColumnType("NUMBER(10,3)")
                .HasColumnName("MONTOTOTAL");

            entity.HasOne(d => d.IdclientNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.Idclient)
                .HasConstraintName("FKCLIENTE");
        });
        modelBuilder.HasSequence("CLIENTESEQ");
        modelBuilder.HasSequence("DVTASEQ");
        modelBuilder.HasSequence("PRODUCTOSEQ");
        modelBuilder.HasSequence("VENTASEQ");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
