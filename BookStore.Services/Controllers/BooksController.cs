using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Services.Models;
using Microsoft.AspNetCore.Authorization;
using BookStore.Repo;

namespace BookStore.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookStoreContext _context;
        private readonly IBookRepo _repo;


        public BookController(BookStoreContext context, IBookRepo repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/Books
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetBook()
        {

            var sdoighsdo = _repo.InsertBook();
            //var emailAddresss = HttpContext.User.Identity.Name;
            var books = _context.Book.Include(a => a.Pub).ToList();


            //return books;
            return Ok(books);
            //return await _context.Book.Include(a=>a.BookAuthors).ToListAsync();
        }


        [HttpGet]
        [Route("Books")]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            return _context.Book.ToList();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = _context.Book.Include(a=>a.Pub).Where(a=>a.BookId==id).FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
