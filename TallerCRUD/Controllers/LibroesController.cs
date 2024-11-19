using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TallerCRUD.Models;

namespace TallerCRUD.Controllers
{
    public class LibroesController : Controller
    {
        private readonly CrudTallerContext _context;

        public LibroesController(CrudTallerContext context)
        {
            _context = context;
        }

        // GET: Libroes
        public async Task<IActionResult> Index(string filtrar)
        {
            var libros = from Libro in _context.Libros select Libro;
            ViewData["FiltroNombre"] = String.IsNullOrEmpty(filtrar) ? "NombreDescendente" : "";
            ViewData["FiltroPublicacion"] = filtrar == "FechaAscendente" ? "FechaDescendente" : "FechaAscendente";
            switch (filtrar)
            {
                case "NombreDescendente":
                    libros = libros.OrderByDescending(libro => libro.Titulo);
                    break;
                case "FechaDescendente":
                    libros = libros.OrderByDescending(libro => libro.Publicacion);
                    break;
                case "FechaAscendente":
                    libros = libros.OrderBy(libro => libro.Publicacion);
                    break;
                default:
                    libros = libros.OrderByDescending(libro => libro.Titulo);
                    break;
            }
            var crudTallerContext = _context.Libros.Include(l => l.CodigoCategoriaNavigation).Include(l => l.NitEditorialNavigation);
            return View(await crudTallerContext.ToListAsync());
        }

        // GET: Libroes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.CodigoCategoriaNavigation)
                .Include(l => l.NitEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libroes/Create
        public IActionResult Create()
        {
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria");
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit");
            return View();
        }

        // POST: Libroes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn,Titulo,Descripcion,NombreAutor,Publicacion,FechaRegistro,CodigoCategoria,NitEditorial")] Libro libro)
        {

            if (_context.Libros.Any(l => l.Isbn == libro.Isbn))
            {
                // ISBN ya existe
                TempData["ErrorMessage"] = "El ISBN ya existe. No se puede crear el libro.";
                ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
                ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
                return View(libro);
            }
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
            return View(libro);
        }

        // GET: Libroes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria");
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit");
            return View(libro);
        }

        // POST: Libroes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Isbn,Titulo,Descripcion,NombreAutor,Publicacion,FechaRegistro,CodigoCategoria,NitEditorial")] Libro libro)
        {
            if (id != libro.Isbn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Isbn))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria");
                ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit");
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // GET: Libroes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libroes/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                TempData["ErrorMessage"] = "Libro no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_"))
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el libro porque tiene autores asociados.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al eliminar el libro.";
                }
                return RedirectToAction(nameof(Index));

            }

        }

        private bool LibroExists(string id)
        {
            return _context.Libros.Any(e => e.Isbn == id);
        }
    }
}