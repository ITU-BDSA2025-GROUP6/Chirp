using System.Diagnostics;

namespace End2End;

public class LocalHostServer : IDisposable

{
    private Process? _process;

    public async Task StartAsync()
    {
        string projectPath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "src", "Chirp.Web", "Chirp.Web.csproj"));

        var info = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{projectPath}\" --urls=https://localhost:5273",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        
        _process = Process.Start(info);
        await WaitAsync("https://localhost:5273");

    }

    private async Task WaitAsync(string url)
    {
        using var client = new HttpClient();
        var timeout = TimeSpan.FromSeconds(60); 
        var start = DateTime.Now;

        while (DateTime.Now - start < timeout)
        {
            try
            {
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                    return;
            }
            catch
            {
                
            }
            
            await Task.Delay(500);
        }
        
        throw new TimeoutException("The server timed out");
    }

    public void Dispose()
    {
        if (!_process.HasExited)
        {
            _process.Kill(true);
            _process.WaitForExit();
        }
        _process.Dispose();
    }
}