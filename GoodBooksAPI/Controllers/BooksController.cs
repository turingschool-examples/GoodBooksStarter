using GoodBooksAPI.DataAccess;
using GoodBooksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodBooksAPI.Controllers
{
    // This will be the default route - controller gets replaced with the name of the controller.
    // "/api/books"
    [Route("/api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private GoodBooksApiContext _context;

        public BooksController(GoodBooksApiContext context)
        {
            _context = context;
        }

        // GET "/api/books"
        [HttpGet]
        public ActionResult GetBooks()
        {
            var books = _context.Books.ToList();

            // We could manually set the response code like this:
            //Response.StatusCode = 200;

            // API endpoints should return JSON, we are creating a new JSON result with our list of books.
            return new JsonResult(books);
        }

        // GET "/api/books/:id"
        [HttpGet("{id}")]
        public ActionResult GetBook(int id)
        {
            var book = _context.Books.Find(id);

            if (book is null)
            {
                return NotFound();
            }

            return new JsonResult(book);
        }

        [HttpPost]
        public void CreateBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return;
            }
            _context.Books.Add(book);
            _context.SaveChanges();

            Response.StatusCode = 201;

            return;
        }

        [HttpPut("{id}")]
        public void UpdateBook(int id, Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();

            Response.StatusCode = 204;

            return;
        }

        [HttpDelete("{id}")]
        public void DeleteBook(int id)
        {
            Book book = _context.Books.Find(id);
            _context.Books.Remove(book);
            _context.SaveChanges();

            Response.StatusCode = 204;

            return;
        }
    }
}
