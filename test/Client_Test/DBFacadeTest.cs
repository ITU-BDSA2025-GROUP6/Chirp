using System.Data;
using Chirp.Razor;
using Microsoft.Data.Sqlite;
using Xunit.Abstractions;
using Xunit;
namespace Client_Test;


public class DBFacadeTest
{
    [Fact]
    // Testing that a connection can be established to our database (chirp.db)
    public void ConnectionTest () 
    {
        //Arrange
        DBFacade test = new DBFacade();
        
        //Act
        SqliteConnection _connection = test.GetConnection();
        
        // var sqlQuery = @"SELECT * FROM message";
        // var command = _connection.CreateCommand();
        // Console.WriteLine(command.CommandText = sqlQuery);
        
        //Assert
        Assert.NotNull(_connection);
        Assert.Equal(ConnectionState.Open, _connection.State);
    }

    [Fact]
    // should reference a local database made specifically for test
    public void getAll()
    {
        CheepService cheepService = new CheepService();
        List<CheepViewModel> list = cheepService.GetCheeps();
        Console.WriteLine(list.Count);
        Assert.True(list.Count == 2);
    }
}