using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;

namespace End2End;

[Parallelizable(ParallelScope.Self)]
[TestFixture] 
public class Tests : PageTest
// Terminal command: pwsh bin/Debug/net8.0/playwright.ps1 codegen  https://localhost:5273 
{
    private const string Url = "http://localhost:5273"; 
    //private const string Url = "https://bdsa2025group6chirp.azurewebsites.net/";
    private LocalHostServer? _server;
    
    [OneTimeSetUp]
    public async Task Setup()
    {
        _server = new LocalHostServer();
        await _server.StartAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        if (_server != null)
            await _server.DisposeAsync();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
    
    [Test]
    public async Task HasTitle()
    {
        await Page.GotoAsync(Url);

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Chirp!"));
    }
    
    [Test]
    public async Task GetStartedLink()
    {
        await Page.GotoAsync(Url);

        // Click the get started link.
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();

        // Expects page to have a heading with the name of Installation.
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Register", Exact = true})).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task GoToRegisterPage()
    {
        await Page.GotoAsync(Url);
        await Page.ClickAsync("text=Register");
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = "register.png"
        });

        var isExist = await Page.Locator("text=Use another service to register.").IsVisibleAsync();
        Assert.That(isExist, Is.True);
    }
    
    // EXAMPLE TEST CASE FROM SESSION_09: 
    [Test]
    public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
    {
        await Page.GotoAsync("https://playwright.dev");
        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        // create a locator
        var getStarted = Page.GetByRole(AriaRole.Link, new() { Name = "Get started" });
        // Expect an attribute "to be strictly equal" to the value.
        await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

        // Click the get started link.
        await getStarted.ClickAsync();
        // Expects the URL to contain intro.
        await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
    }
}