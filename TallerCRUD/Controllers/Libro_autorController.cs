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
    public class Libro_autorController : Controller
    {
        private readonly CrudTallerContext _context;

        public Libro_autorController(CrudTallerContext context)
        {
            _context = context;
        }

        // GET: Libro_autor
        public async Task<IActionResult> Index()
        {
            var crudTallerContext = _context.Libro_autores.Include(l => l.IdAutorNavigation).Include(l => l.IsbnLibroNavigation);
            return View(await crudTallerContext.ToListAsync());
        }

        // GET: Libro_autor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro_autor = await _context.Libro_autores
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IsbnLibroNavigation)
                .FirstOrDefaultAsync(m => m.IdAutor == id);
            if (libro_autor == null)
            {
                return NotFound();
            }

            return View(libro_autor);
        }

        // GET: Libro_autor/Create
        public IActionResult Create()
        {
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor");
            ViewData["Isbn"] = new SelectList(_context.Libros, "Isbn", "Isbn");
            return View();
        }

        // POST: Libro_autor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAutor,Isbn")] Libro_autor libro_autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro_autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor", libro_autor.IdAutor);
            ViewData["Isbn"] = new SelectList(_context.Libros, "Isbn", "Isbn", libro_autor.Isbn);
            return View(libro_autor);
        }

        // GET: Libro_autor/Edit/5
        public async Task<IActionResult> Edit(int? idAutor, string? isbn)
        {
            if (idAutor == null || isbn == null)
            {
                return NotFound();
            }

            var libro_autor = await _context.Libro_autores.FindAsync(idAutor, isbn);
            if (libro_autor == null)
            {
                return NotFound();
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor", libro_autor.IdAutor);
            ViewData["Isbn"] = new SelectList(_context.Libros, "Isbn", "Isbn", libro_autor.Isbn);
            return View(libro_autor);
        }
        // GET: Libro_autor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? idAutor, string? isbn)
        {
            if (idAutor == null || isbn == null)
            {
                return NotFound();
            }

            var libro_autor = await _context.Libro_autores
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IsbnLibroNavigation)
                .FirstOrDefaultAsync(m => m.IdAutor == idAutor && m.Isbn == isbn);
            if (libro_autor == null)
            {
                return NotFound();
            }

            return View(libro_autor);
        }

        // POST: Libro_autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? idAutor, string? isbn)
        {
            var libro_autor = await _context.Libro_autores.FindAsync(idAutor, isbn);
            if (libro_autor != null)
            {
                _context.Libro_autores.Remove(libro_autor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Libro_autorExists(int id)
        {
            return _context.Libro_autores.Any(e => e.IdAutor == id);
        }
    }
}
