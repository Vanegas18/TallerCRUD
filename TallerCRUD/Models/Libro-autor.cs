using System;
using System.Collections.Generic;

namespace TallerCRUD.Models
{
    public class Libro_autor
    {
        public int IdAutor { get; set; }
        public string? Isbn { get; set; }

        public virtual Autor? IdAutorNavigation { get; set; }
        public virtual Libro? IsbnLibroNavigation { get; set; }
    }
}
