using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class AuthorRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AuthorRepository _repository;
    private readonly CheepDbContext _context;

    public AuthorRepositoryTests()
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

    public class CreateAuthorTests : AuthorRepositoryTests
    {
        
    }

    public class GetAuthorTests : AuthorRepositoryTests
    {
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

        [Fact]
        public async Task GetAuthorByName_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nameNotInDatabase = "username";
            InvalidOperationException? exception = null;
            
            // Act
            try
            {
                await _repository.GetAuthorByName(nameNotInDatabase);

            }
            catch (InvalidOperationException e)
            {
                exception = e;
            }
            
            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal($"No such author with name: {nameNotInDatabase}", exception.Message);
        }

        [Fact]
        public async Task GetAuthorByEmail_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var emailNotInDatabase = "email@email.com";
            InvalidOperationException? exception = null;
            
            // Act
            try
            { 
                await _repository.GetAuthorByEmail(emailNotInDatabase);

            }
            catch (InvalidOperationException e)
            {
                exception = e;
            }
            
            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal($"No such author with email: {emailNotInDatabase}", exception.Message);
        }
        
        [Fact]
        public async Task GetAuthorByName_ShouldThrow_WhenNameIsNull()
        {
            // Arrange
            string? userName = null;
            InvalidOperationException? exception = null;

            // Act
            try
            {
                await _repository.GetAuthorByName(userName);
            }
            catch (InvalidOperationException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal("Name cannot be null or empty", exception.Message);
        }

        [Fact]
        public async Task GetAuthorByEmail_ShouldThrow_WhenEmailIsNull()
        {
            string? email = null;
            InvalidOperationException? exception = null;
            
            // Act
            try
            {
                await _repository.GetAuthorByEmail(email);
            }
            catch (InvalidOperationException ex)
            {
                exception = ex;
            }
            
            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal("Email cannot be null or empty", exception.Message);
        }
    }
}