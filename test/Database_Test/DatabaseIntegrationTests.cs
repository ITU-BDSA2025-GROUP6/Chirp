using System;
using System.IO;
using Xunit;
using SimpleDB;
using Chirp.CLI; 

public class DatabaseIntegrationTests
{
    private readonly string _dbFilePath = "../SimpleDB/Data/Cheep_DB.csv";

    private void ResetDatabaseFile()
    {
        if (File.Exists(_dbFilePath))
        {
            File.Delete(_dbFilePath);
        }
    }

    [Fact]
    public void StoreAndRead_Cheep_ShouldBePersisted()
    {
        // Arrange
        ResetDatabaseFile();
        var db = CsvDatabase<Cheep>.Instance!;
        var cheep = new Cheep("testuser", "Hello integration test!", DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        // Act
        db.Store(cheep);
        var cheeps = db.Read();

        // Assert
        Assert.Contains(cheeps, c =>
            c.Author == cheep.Author &&
            c.Message == cheep.Message &&
            c.Timestamp == cheep.Timestamp
        );
    }

    [Fact]
    public void Read_EmptyDatabase_ShouldReturnEmptyList()
    {
        // Arrange
        ResetDatabaseFile();
        var db = CsvDatabase<Cheep>.Instance!;

        // Act
        var cheeps = db.Read();

        // Assert
        Assert.Empty(cheeps);
    }

    [Fact]
    public void Store_MultipleCheeps_ShouldBeRetrievedInOrder()
    {
        // Arrange
        ResetDatabaseFile();
        var db = CsvDatabase<Cheep>.Instance!;

        var cheep1 = new Cheep("alice", "First cheep", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        var cheep2 = new Cheep("bob", "Second cheep", DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        // Act
        db.Store(cheep1);
        db.Store(cheep2);
        var cheeps = db.Read();

        // Assert
        Assert.Collection(cheeps,
            c => Assert.Equal("First cheep", c.Message),
            c => Assert.Equal("Second cheep", c.Message));
    }
}