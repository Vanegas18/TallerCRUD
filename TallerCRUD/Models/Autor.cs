using System;
using System.Collections.Generic;

namespace TallerCRUD.Models;

public partial class Autor
{
    public int IdAutor { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Nacionalidad { get; set; }

    //public virtual ICollection<Libro> Isbns { get; set; } = new List<Libro>();
    public virtual ICollection<Libro_autor> Libro_autores { get; set; } = new List<Libro_autor>();
}
