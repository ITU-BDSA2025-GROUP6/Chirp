using Microsoft.Playwright;

namespace End2End
{
    public class RegisterAndLogInTest
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private LocalHostServer? _server;
        private const string Url = "http://localhost:5273";

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _server = new LocalHostServer();
            await _server.StartAsync();
            
            _playwright = await Playwright.CreateAsync();

            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
            });
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            if (_browser != null) await _browser.CloseAsync();
            _playwright?.Dispose();
            
            if(_server != null) await _server.DisposeAsync();
        }
        
        private async Task<IPage> CreatePageAsync()
        {
            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true // <-- important for self-signed dev cert
            });

            return await context.NewPageAsync();
        }

        [Test, Order(1)]
        public async Task RegisterAndLogin()
        {
            var page = await CreatePageAsync();
            
            await page.GotoAsync(Url);
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
        public async Task LoginAndLogout()
        {
            var page = await CreatePageAsync();
            
            await page.GotoAsync(Url);
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("testemail@example.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await page.GetByRole(AriaRole.Heading, new() { Name = "My Timeline - Page" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Account" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Personal data" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Logout [testemail@example.com]" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
            await page.GetByText("Public Timeline | Register |").ClickAsync();
        }

        [Test, Order(3)]
        public async Task LoginAndFollowAndUnfollow()
        {
            var page = await CreatePageAsync();
            await page.GotoAsync(Url);
            
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("testemail@example.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = "Mellie Yost Follow But what" }).GetByRole(AriaRole.Link).Nth(1).ClickAsync();
            await page.GetByText("But what was behind the").ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await page.GetByText("But what was behind the").ClickAsync();
            await page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = "Mellie Yost He glared from" }).Locator("div").Nth(2).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Unfollow" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await page.GetByText("There are no cheeps so far.").ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Logout [testemail@example.com]" }).ClickAsync();
        }


        [Test, Order(4)]
        public async Task LoginAndRecheepAndRemoveRecheep()
        {
            var page = await CreatePageAsync();
            await page.GotoAsync(Url);
            
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("testemail@example.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Qwerty123!");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await page.GetByText("There are no cheeps so far.").ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
            await page.GetByText("I wonder if he''d give a very").ClickAsync();
            await page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = "Jacqualine Gilcoine Follow I wonder if he''d give a very shiny top hat and my" }).GetByRole(AriaRole.Button).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await page.GetByText("I wonder if he''d give a very").ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
            await page.GetByText("I wonder if he''d give a very").ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Remove Recheep" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await page.GetByText("There are no cheeps so far.").ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Logout [testemail@example.com]" }).ClickAsync();


        }

        [Test, Order(5)]
        public async Task LoginAndDeleteAccount()
        {
            var page = await CreatePageAsync();
            
            await page.GotoAsync(Url);
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
    }
}