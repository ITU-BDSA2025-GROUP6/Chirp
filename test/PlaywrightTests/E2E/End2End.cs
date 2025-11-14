using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using var playwright = await Playwright.CreateAsync();

await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
    {
        Headless = false,
    });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("http://localhost:5273/");
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

 