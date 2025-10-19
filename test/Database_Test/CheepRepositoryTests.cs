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