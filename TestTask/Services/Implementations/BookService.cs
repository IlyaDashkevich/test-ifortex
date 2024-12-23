using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class BookService : IBookService
{
    private readonly ApplicationDbContext _context;

    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }
    public Task<Book> GetBook()
    {
        var book = _context.Books
            .OrderByDescending(b => b.Price * b.QuantityPublished)
            .FirstOrDefault();

        return Task.FromResult(book);
    }

    public Task<List<Book>> GetBooks()
    {
        var carolusRexReleaseDate = new DateTime(2012, 4, 6); 

        var books = _context.Books
            .Where(b => b.Title.Contains("Red") && b.PublishDate > carolusRexReleaseDate)
            .ToList();

        return Task.FromResult(books);
    }
}