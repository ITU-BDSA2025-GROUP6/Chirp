using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
        Assert.Equal("Test Author", createdCheep.Author?.UserName);
        Assert.Equal("Hello, this is a test cheep!", createdCheep.Text);
    }

    [Fact]
    public async Task CreateCheep_ShouldThrowExecptionIfCheepTooLong()
    {
        //Arrange
        var invalidCheep = new CheepDTO
        {
            AuthorName = "Test Author",
            Text = new string('*', 200),
            Timestamp = DateTime.UtcNow
        };

        var validCheep = new CheepDTO
        {
            AuthorName = "Test Author",
            Text = new string('*', 160),
            Timestamp = DateTime.UtcNow
        };
        
        
        var resultId = await _repository.CreateCheep(validCheep);
        Assert.True(resultId > 0);

        await Assert.ThrowsAsync<ValidationException>(async () => await _repository.CreateCheep(invalidCheep));
    }

    [Fact]
    public async Task CreateCheep_ShouldThrowExceptionIfAuthorNotFound()
    {
        var cheep = new CheepDTO
        {
            AuthorName = "No Author",
            Text = "Hello, this is a test cheep!",
            Timestamp = DateTime.UtcNow
        };
        
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _repository.CreateCheep(cheep));
    }

    [Fact]
    public async Task CreateCheep_ShouldSetTimestampIfNotProvided()
    {
        var cheep = new CheepDTO
        {
            AuthorName = "Test Author",
            Text = "Hello, this is a test cheep!",
        };
        
        var resultId = await _repository.CreateCheep(cheep);
        var savedCheep = await _context.Cheeps.FindAsync(resultId);
        
        Assert.NotNull(savedCheep);
        Assert.True(savedCheep.Timestamp < DateTime.UtcNow);
        Assert.True(DateTime.UtcNow - savedCheep.Timestamp < TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task CreateCheep_ShouldHandleProvidedCheepId()
    {
        var cheep = new CheepDTO
        {
            CheepID = 100,
            AuthorName = "Test Author",
            Text = "Hello, this is a test cheep!",
            Timestamp = DateTime.UtcNow
        };
        var resultId = await _repository.CreateCheep(cheep);
        Assert.True(resultId > 0);
        Assert.NotEqual(0, resultId);
        Assert.Equal(100, resultId);
    }

    [Fact]
    public async Task CreateCheep_ShouldThrowExceptionIfCheepIdAlreadyExists()
    {
        // Arrange
        var cheep1 = new CheepDTO
        {
            CheepID = 1,
            AuthorName = "Test Author",
            Text = "Hello, this is a test cheep!",
        };
        var cheep2 = new CheepDTO
        {
            CheepID = 1,
            AuthorName = "Test Author",
            Text = "Hello, this is a test cheep!",
        };
        
        // Act
        await _repository.CreateCheep(cheep1);
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _repository.CreateCheep(cheep2));
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
        /*
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
        */
    }

    [Fact]
    public async Task GetAuthorByEmail_ShouldReturnAuthor()
    {
        /*
        //Arrange
        var author = await _repository.GetAuthorByEmail("test@author.com");
        // var author2 = await _repository.GetAuthorByEmail("test2@email.com");
        
        Assert.NotNull(author);
        Assert.Equal("1", author.Id);
        Assert.Equal("Test Author", author.Name);
        Assert.Equal("test@author.com", author.Email);
        Assert.NotNull(author.Cheeps);
        */
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
