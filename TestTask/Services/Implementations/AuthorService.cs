using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly ApplicationDbContext _context;

    public AuthorService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<List<Author>> GetAuthors()
    {
        var authors = _context.Authors
            .Select(author => new
            {
                Author = author,
                BookCount = _context.Books.Count(book => book.AuthorId == author.Id && book.PublishDate > new DateTime(2015, 1, 1))
            })
            .Where(a => a.BookCount > 0 && a.BookCount % 2 == 0) 
            .Select(a => a.Author) 
            .ToList();

        return Task.FromResult(authors);
    }
    
    public Task<Author> GetAuthor()
    {
        var authorWithLongestTitle = _context.Books
            .OrderByDescending(book => book.Title.Length)
            .ThenBy(book => book.AuthorId) 
            .Select(book => new { book.AuthorId })
            .FirstOrDefault();

        if (authorWithLongestTitle == null)
            return null;

        var author = _context.Authors
            .FirstOrDefault(a => a.Id == authorWithLongestTitle.AuthorId);

        return Task.FromResult(author);
    }
}