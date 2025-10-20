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
        
        var author = new Author { AuthorID = 1, Name = "Test Author", Email = "test@author.com", Cheeps = new List<Cheep>() };
        _context.Authors.Add(author);
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
        Assert.Equal("Test Author", createdCheep.Author.Name);
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
    
    public void Dispose()
    {
        // this method is called after all tests in the class have run
        // disposes of connection
        _connection.Close();
        _connection.Dispose();
        _context.Dispose();
    }
}