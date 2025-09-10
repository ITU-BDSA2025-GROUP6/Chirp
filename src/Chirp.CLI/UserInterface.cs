namespace Chirp.CLI;

/// <summary>
/// Provides methods for user interaction within the Chirp CLI application.
/// </summary>
public static class UserInterface
{
    /// <summary>
    /// Prints a collection of Cheep objects to the console.
    /// Each Cheep is displayed with the author's username, the timestamp of the message,
    /// and the message content.
    /// </summary>
    /// <param name="cheeps">The collection of Cheep objects to display.</param>
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            Console.WriteLine(
                $"{cheep.user_name} @ {DateTimeOffset.FromUnixTimeSeconds(cheep.unixTimeStamp).DateTime} : {cheep.user_message}  ");
        }
    }
}