﻿using System;
using System.Collections.Generic;

namespace TallerCRUD.Models;

public partial class Libro
{
    public string Isbn { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? NombreAutor { get; set; }

    public DateOnly? Publicacion { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public int? CodigoCategoria { get; set; }

    public int? NitEditorial { get; set; }

    public virtual Categoria? CodigoCategoriaNavigation { get; set; }

    public virtual Editoriale? NitEditorialNavigation { get; set; }

    //public virtual ICollection<Autor> IdAutors { get; set; } = new List<Autor>();
    public virtual ICollection<Libro_autor> Libro_autores { get; set; } = new List<Libro_autor>();
}
