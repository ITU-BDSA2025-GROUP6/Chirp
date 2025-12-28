using Microsoft.Playwright;

namespace End2End
{
    public class RegisterAndLogInTest
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private LocalHostServer _server;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _server = new LocalHostServer();
            await _server.StartAsync();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _server.Dispose();
        }
        
        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();

            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
            });
        }

        [TearDown]
        public async Task TearDown()
        {
            if (_browser != null) { await _browser.CloseAsync(); }
            _playwright.Dispose();
        }

        [Test, Order(1)]
        public async Task RegisterAndLogin()
        {
            var context = await _browser.NewContextAsync();
            var page = await context.NewPageAsync();
            
            await page.GotoAsync("https://localhost:5273/");
            await page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("testemail@example.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password" }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("testemail@example.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Logout [testemail@example.com]" }).ClickAsync();
        }

        [Test, Order(2)]
        public async Task LoginAndDeleteAccount()
        {
            var context = await _browser.NewContextAsync();
            var page = await context.NewPageAsync();
            
            await page.GotoAsync("https://localhost:5273/");
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("testemail@example.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Account" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Personal data" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();

        }

        /*
        [Test]
        public async Task RegisterAndLogIn2()
        {
            var context = await _browser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync("https://localhost:5273/");
            /* 
            User already registered !!! - CHECK issue #139 !!!
            ¨
            await page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("testemail@example.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password" }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
            
            
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("testemail@example.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await page.Locator("#Text").ClickAsync();
            await page.Locator("#Text").FillAsync("cheep");
            await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
        }
        */
    }

}