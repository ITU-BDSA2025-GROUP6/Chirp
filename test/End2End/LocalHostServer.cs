using System.Diagnostics;

namespace End2End;

public class LocalHostServer : IAsyncDisposable

{
    private Process? _process;

    public async Task StartAsync()
    {
        string projectPath = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory, 
                "..", "..", "..", "..", "..", "src", "Chirp.Web", "Chirp.Web.csproj"
            )
        );
        
        if (!File.Exists(projectPath))
            throw new FileNotFoundException("Could not find project file", projectPath);

        var info = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{projectPath}\" --urls=http://localhost:5273",
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = true
        };
        
        _process = Process.Start(info)
            ?? throw new InvalidOperationException("Could not start the server");
        
        await WaitAsync("http://localhost:5273");

    }

    private async Task WaitAsync(string url)
    {
        using var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        
        using var client = new HttpClient(handler);
        var timeout = TimeSpan.FromSeconds(60); 
        var start = DateTime.Now;

        while (DateTime.Now - start < timeout)
        {
            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    await Task.Delay(500);
                    return;
                }
            }
            catch
            {
                // ignored
            }

            await Task.Delay(500);
        }
        
        throw new TimeoutException("The server timed out");
    }

    public async ValueTask DisposeAsync()
    {
        if (_process == null) return;
        try
        {
            if (!_process.HasExited)
            {
                _process.Kill(entireProcessTree: true);
                await _process.WaitForExitAsync();
            }
        }
        catch
        {
            // ignored
        }
        finally
        {
            _process.Dispose();
        }
    }
}