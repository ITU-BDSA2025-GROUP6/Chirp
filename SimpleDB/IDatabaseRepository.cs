namespace SimpleDB;

/// <summary>
/// Defines essential methods for interacting with a database to perform storage and retrieval operations
/// for entities of a specified type.
/// </summary>
/// <typeparam name="T">
/// The type of the objects that the repository will handle.
/// </typeparam>
public interface IDatabaseRepository<T>
{
    IEnumerable<T> Read(int? limit = null);
    void Store(T record);
}