using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;

public class AuthorRepositoryIntegrationTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AuthorRepository _repository;
    private readonly CheepDbContext _context;
    
    public AuthorRepositoryIntegrationTests() 
    {
    _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<CheepDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new CheepDbContext(options);
        _context.Database.EnsureCreated();

        var author = new Author
            { Id = "1", UserName = "Test Author", Email = "test@author.com", Cheeps = new List<Cheep>() };
        var author2 = new Author
            { Id = "2", UserName = "Test Author2", Email = "test2@email.com", Cheeps = new List<Cheep>() };

        //_context.Authors.Add(author);
        //_context.Authors.Add(author2);
        _context.Authors.AddRange(author, author2);
        _context.SaveChanges();

        _repository = new AuthorRepository(_context);
    }
    
    public void Dispose()
    {
        // this method is called after all tests in the class have run
        // disposes of connection
        _connection.Close();
        _connection.Dispose();
        _context.Dispose();
    }
    
    public class AuthorTests : AuthorRepositoryIntegrationTests
    {

        [Fact]
        public async Task GetAuthorByName_retrievesCorrectAuthorOrThrows()
        {
            var authorName = "Test Author";
            //act
            var dto = await _repository.GetAuthorByName(authorName);

            //assert
            Assert.NotNull(dto);
            Assert.Equal("1", dto.Id);
            Assert.Equal("Test Author", dto.Name);
            Assert.Equal("test@author.com", dto.Email);
        }
    }
}