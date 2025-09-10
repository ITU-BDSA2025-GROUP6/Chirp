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
            var when = DateTimeOffset.FromUnixTimeSeconds(cheep.unixTimeStamp).ToLocalTime();
            Console.WriteLine(
                $"{cheep.user_name} @ {when:dd-MM-yyyy HH:mm} : {cheep.user_message}  ");
        }
    }
}