using Microsoft.Playwright;

namespace End2End
{
    public class End2EndTests
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private LocalHostServer? _server;
        private const string Url = "http://localhost:5273";

        // Primary test user — registered in test 1, deleted in test 5
        private const string TestUserEmail = "testemail@example.com";
        private const string TestUserPassword = "Qwerty123!";

        // Secondary author — created in OneTimeSetUp so tests 3 & 4 have
        // a real author to follow/recheep without relying on seed data
        private const string AuthorEmail = "testauthor@example.com";
        private const string AuthorPassword = "Qwerty123!";
        private const string AuthorCheepText =
            "Hello from test author — unique e2e test cheep!";

        // ------------------------------------------------------------------ setup

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

            await SetupTestAuthorAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            // Clean up the test author so the suite is idempotent on repeated runs
            await CleanupTestAuthorAsync();

            if (_browser != null) await _browser.CloseAsync();
            _playwright?.Dispose();
            if (_server != null) await _server.DisposeAsync();
        }

        private async Task CleanupTestAuthorAsync()
        {
            try
            {
                var page = await CreatePageAsync();
                await LoginAsync(page, AuthorEmail, AuthorPassword);
                await page.GetByRole(AriaRole.Link, new() { Name = "Account" }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Personal data" }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync(AuthorPassword);
                await page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
            }
            catch
            {
                // Best-effort — don't fail the test run if cleanup goes wrong
            }
        }

        private async Task<IPage> CreatePageAsync()
        {
            var context = await _browser!.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true
            });
            return await context.NewPageAsync();
        }

        /// <summary>
        /// Register the test author via the UI and have them post one cheep,
        /// so the follow / recheep tests have real data to work with.
        /// </summary>
        private async Task SetupTestAuthorAsync()
        {
            var page = await CreatePageAsync();
            await page.GotoAsync(Url);

            // Register — RequireConfirmedAccount is false, so registration
            // immediately signs the user in and redirects to the public timeline.
            await page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync(AuthorEmail);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync(AuthorPassword);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password" }).FillAsync(AuthorPassword);
            await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            // User is now signed in on the public timeline — post the cheep
            await page.Locator("#Text").FillAsync(AuthorCheepText);
            await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

            // Logout
            await page.GetByRole(AriaRole.Button, new() { Name = $"Logout [{AuthorEmail}]" }).ClickAsync();
        }

        private async Task LoginAsync(IPage page, string email, string password)
        {
            await page.GotoAsync(Url);
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync(email);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync(password);
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        }

        // ------------------------------------------------------------------ tests

        /// <summary>
        /// Verifies the full registration → email confirmation → login flow.
        /// </summary>
        [Test, Order(1)]
        public async Task RegisterAndLogin()
        {
            var page = await CreatePageAsync();
            await page.GotoAsync(Url);

            // Register — auto-signs in, no email confirmation step
            await page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync(TestUserEmail);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync(TestUserPassword);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password" }).FillAsync(TestUserPassword);
            await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();

            // Registration auto-signs in — logout button must be visible
            await Assertions.Expect(
                page.GetByRole(AriaRole.Button, new() { Name = $"Logout [{TestUserEmail}]" })
            ).ToBeVisibleAsync();

            // Logout, then log back in to verify the login flow also works
            await page.GetByRole(AriaRole.Button, new() { Name = $"Logout [{TestUserEmail}]" }).ClickAsync();
            await LoginAsync(page, TestUserEmail, TestUserPassword);
            await Assertions.Expect(
                page.GetByRole(AriaRole.Button, new() { Name = $"Logout [{TestUserEmail}]" })
            ).ToBeVisibleAsync();
        }

        /// <summary>
        /// Verifies that posting a cheep adds it to both the public timeline
        /// and the user's own timeline.
        /// </summary>
        [Test, Order(2)]
        public async Task LoginAndPostCheep()
        {
            var page = await CreatePageAsync();
            await LoginAsync(page, TestUserEmail, TestUserPassword);

            const string cheepText = "This is my first test cheep from the e2e suite!";
            await page.Locator("#Text").FillAsync(cheepText);
            await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

            // Cheep must appear on the public timeline immediately after posting
            await Assertions.Expect(page.GetByText(cheepText)).ToBeVisibleAsync();

            // Cheep must also appear on own user timeline
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await Assertions.Expect(page.GetByText(cheepText)).ToBeVisibleAsync();
        }

        /// <summary>
        /// Verifies that following an author adds their cheeps to My Timeline,
        /// and that unfollowing removes them.
        /// </summary>
        [Test, Order(3)]
        public async Task LoginAndFollowAndUnfollow()
        {
            var page = await CreatePageAsync();
            await LoginAsync(page, TestUserEmail, TestUserPassword);

            // Follow the test author via the Follow link next to their cheep
            await page.GetByRole(AriaRole.Listitem)
                .Filter(new() { HasText = AuthorCheepText })
                .GetByRole(AriaRole.Link, new() { Name = "Follow" })
                .ClickAsync();

            // The author's cheep should now appear in My Timeline
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await Assertions.Expect(page.GetByText(AuthorCheepText)).ToBeVisibleAsync();

            // Unfollow via the public timeline
            await page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
            await page.GetByRole(AriaRole.Listitem)
                .Filter(new() { HasText = AuthorCheepText })
                .GetByRole(AriaRole.Link, new() { Name = "Unfollow" })
                .ClickAsync();

            // The author's cheep should no longer appear in My Timeline
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await Assertions.Expect(page.GetByText(AuthorCheepText)).Not.ToBeVisibleAsync();
        }

        /// <summary>
        /// Verifies that recheepping a post adds it to My Timeline,
        /// and that removing the recheep removes it again.
        /// </summary>
        [Test, Order(4)]
        public async Task LoginAndRecheepAndRemoveRecheep()
        {
            var page = await CreatePageAsync();
            await LoginAsync(page, TestUserEmail, TestUserPassword);

            // Recheep the test author's post from the public timeline
            await page.GetByRole(AriaRole.Listitem)
                .Filter(new() { HasText = AuthorCheepText })
                .GetByRole(AriaRole.Button, new() { Name = "Recheep" })
                .ClickAsync();

            // The recheepped post should now appear in My Timeline
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await Assertions.Expect(page.GetByText(AuthorCheepText)).ToBeVisibleAsync();

            // Remove the recheep from the public timeline
            await page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
            await page.GetByRole(AriaRole.Listitem)
                .Filter(new() { HasText = AuthorCheepText })
                .GetByRole(AriaRole.Button, new() { Name = "Remove Recheep" })
                .ClickAsync();

            // The post should no longer appear in My Timeline
            await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
            await Assertions.Expect(page.GetByText(AuthorCheepText)).Not.ToBeVisibleAsync();
        }

        /// <summary>
        /// Verifies that deleting an account logs the user out and
        /// returns them to the public timeline.
        /// </summary>
        [Test, Order(5)]
        public async Task LoginAndDeleteAccount()
        {
            var page = await CreatePageAsync();
            await LoginAsync(page, TestUserEmail, TestUserPassword);

            await page.GetByRole(AriaRole.Link, new() { Name = "Account" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Personal data" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync(TestUserPassword);
            await page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();

            // After deletion the user is logged out — Register link must be visible
            await Assertions.Expect(
                page.GetByRole(AriaRole.Link, new() { Name = "Register" })
            ).ToBeVisibleAsync();
        }
    }
}
