using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Database_Test;

public class CheepRepository_Tests : IDisposable
{

    private readonly SqliteConnection _connection;
    private readonly CheepRepository _repository;
    private readonly CheepDBContext _context;

    public CheepRepository_Tests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        
        var options = new  DbContextOptionsBuilder<CheepDBContext>()
            .UseSqlite(_connection)
            .Options;
        
        _context = new CheepDBContext(options);
        _context.Database.EnsureCreated();
        
        var author = new Author { Id = "1", UserName = "Test Author", Email = "test@author.com", Cheeps = new List<Cheep>() };
        var author2 = new Author {  Id = "2", UserName = "Test Author2", Email = "test2@email.com", Cheeps = new List<Cheep>() };
        
        //_context.Authors.Add(author);
        //_context.Authors.Add(author2);
        _context.Authors.AddRange(author, author2);
        _context.SaveChanges();
        
        _repository = new CheepRepository(_context);
    }

    [Fact]
    public async Task createCheep_ShouldAddCheepToDatabase()
    {
        //Arrange
        var newCheep = new CheepDTO
        {
            AuthorName = "Test Author",
            Text = "Hello, this is a test cheep!",
            Timestamp = DateTime.UtcNow
        };

        //Act
        await _repository.CreateCheep(newCheep);
        var createdCheep = await _context.Cheeps.Include(c => c.Author).FirstOrDefaultAsync();
        //Assert
        Assert.NotNull(createdCheep);
        Assert.Equal(1,  createdCheep.CheepID);
        Assert.Equal("Test Author", createdCheep.Author.UserName);
        Assert.Equal("Hello, this is a test cheep!", createdCheep.Text);
    }

    [Fact]
    public async Task CreateCheep_ShouldThrowExecptionIfCheepTooLong()
    {
        
    }

    [Fact]
    public async Task InsertAuthor_ShouldThrowExecptionIfNameNotUnique()
    {
        
    }

/*
    [Fact]    
public async Task CreateAuthorTest_ShouldAddAuthorToDatabase()
    {
        //Arrange
        var newAuthor = new AuthorDTO
        {
            Name = "Test Author2",
            Email = "test@email.com",
        };
        
        //Act
        await _repository.CreateAuthor(newAuthor);
        var createdAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Email == newAuthor.Email);
        
        //Assert
        Assert.NotNull(createdAuthor);
        Assert.NotEqual("1", createdAuthor.Id);
        Assert.Equal("3", createdAuthor.Id);
        Assert.Equal("Test Author2", createdAuthor.UserName);
        Assert.Equal("test@email.com", createdAuthor.Email);
    }
*/

    [Fact]
    public async Task GetAuthorByName_ShouldReturnAuthor()
    {
        //Arrange
        var author = await _repository.GetAuthorByName("Test Author");
        var author2 = await _repository.GetAuthorByName("Test Author2");
        
        Assert.NotNull(author);
        Assert.Equal("Test Author", author.Name);
        Assert.NotEmpty("Test Author");
        Assert.NotNull(author.Cheeps);
        
        Assert.NotNull(author2);
        Assert.Equal("Test Author2", author2.Name);
        Assert.Equal("test2@email.com", author2.Email);
        Assert.NotNull(author2.Cheeps);
    }

    [Fact]
    public async Task GetAuthorByEmail_ShouldReturnAuthor()
    {
        //Arrange
        var author = await _repository.GetAuthorByEmail("test@author.com");
        // var author2 = await _repository.GetAuthorByEmail("test2@email.com");
        
        Assert.NotNull(author);
        Assert.Equal("1", author.Id);
        Assert.Equal("Test Author", author.Name);
        Assert.Equal("test@author.com", author.Email);
        Assert.NotNull(author.Cheeps);
    }
    
    public void Dispose()
    {
        // this method is called after all tests in the class have run
        // disposes of connection
        _connection.Close();
        _connection.Dispose();
        _context.Dispose();
    }
}