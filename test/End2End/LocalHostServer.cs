using System.Diagnostics;

namespace End2End;

public class LocalHostServer : IAsyncDisposable
{
    private Process? _process;

    public async Task StartAsync()
    {
        // In CI the server is started as a separate workflow step — skip if already up
        if (await IsAlreadyRunningAsync())
            return;

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
            Arguments = $"run --project \"{projectPath}\" -c Release --no-build --urls=http://localhost:5273",
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = true
        };

        _process = Process.Start(info)
            ?? throw new InvalidOperationException("Could not start the server");

        await WaitAsync("http://localhost:5273");
    }

    private async Task<bool> IsAlreadyRunningAsync()
    {
        using var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        using var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(3) };
        try
        {
            var response = await client.GetAsync("http://localhost:5273");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private async Task WaitAsync(string url)
    {
        using var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        using var client = new HttpClient(handler);
        var timeout = TimeSpan.FromSeconds(120);
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
        // If _process is null we didn't start the server, so nothing to clean up
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
