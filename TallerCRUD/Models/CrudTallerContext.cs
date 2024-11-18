using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TallerCRUD.Models;

public partial class CrudTallerContext : DbContext
{
    public CrudTallerContext()
    {
    }

    public CrudTallerContext(DbContextOptions<CrudTallerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Editoriale> Editoriales { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Libro_autor> Libro_autores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //        => optionsBuilder.UseSqlServer("Server=localhost;Database=CRUD_TALLER;Integrated Security=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutor).HasName("PK__Autor__3D64C92C5F40DF2F");

            entity.ToTable("Autor");

            entity.Property(e => e.IdAutor).HasColumnName("Id_autor");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.Libro_autores)
                .WithOne(p => p.IdAutorNavigation)
                .HasForeignKey(d => d.IdAutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Libro_auto__Id_a__2E1BDC42");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CodigoCategoria).HasName("PK__Categori__738F04AD72E47827");

            entity.Property(e => e.CodigoCategoria)
                .ValueGeneratedNever()
                .HasColumnName("Codigo_categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Editoriale>(entity =>
        {
            entity.HasKey(e => e.Nit).HasName("PK__Editoria__C7D1D6DBD2FFF08E");

            entity.Property(e => e.Nit).ValueGeneratedNever();
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Sitioweb)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK__Libros__9271CED163CB1FD5");

            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CodigoCategoria).HasColumnName("Codigo_categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnName("Fecha_registro");
            entity.Property(e => e.NitEditorial).HasColumnName("Nit_editorial");
            entity.Property(e => e.NombreAutor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre_autor");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CodigoCategoriaNavigation).WithMany(p => p.Libros)
                .HasForeignKey(d => d.CodigoCategoria)
                .HasConstraintName("FK__Libros__Codigo_c__286302EC");

            entity.HasOne(d => d.NitEditorialNavigation).WithMany(p => p.Libros)
                .HasForeignKey(d => d.NitEditorial)
                .HasConstraintName("FK__Libros__Nit_edit__29572725");
        });

        modelBuilder.Entity<Libro_autor>(entity =>
        {
            entity.HasKey(e => new { e.IdAutor, e.Isbn })
                  .HasName("PK__Libro_au__3443D5C1FD46625A");

            entity.ToTable("Libro_autor");

            entity.Property(e => e.IdAutor)
                  .HasColumnName("Id_autor");

            entity.Property(e => e.Isbn)
                  .HasMaxLength(20)
                  .IsUnicode(false);

            entity.HasOne(d => d.IdAutorNavigation)
                  .WithMany(p => p.Libro_autores)
                  .HasForeignKey(d => d.IdAutor)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK__Libro_auto__Id_a__2E1BDC42");

            entity.HasOne(d => d.IsbnLibroNavigation)
                  .WithMany(p => p.Libro_autores)
                  .HasForeignKey(d => d.Isbn)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK__Libro_auto__Isbn__2F10007B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
