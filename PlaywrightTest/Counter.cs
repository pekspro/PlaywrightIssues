using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTest;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Counter : PageTest
{

    [SetUp]
    public async Task Setup()
    {
        await Context.Tracing.StartAsync(new()
        {
            Title = $"{BrowserName} - {TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}",
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    [TearDown]
    public async Task TearDown()
    {
        // Do not store anything if the test succeeded.
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed)
        {
            return;
        }

        await Context.Tracing.StopAsync(new()
        {
            Path = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "playwright-traces",
                $"{BrowserName} - {TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}.zip"
            )
        });
    }

    [Test]
    public async Task ModifierKeys()
    {
        await Page.GotoAsync("https://firefoxmodifierkeystest.azurewebsites.net/");
        await Expect(Page.Locator("#status")).ToContainTextAsync("Hello, world!", new LocatorAssertionsToContainTextOptions()
        {
            Timeout = 10000
        });

        await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync();
        await Expect(Page.Locator("#status")).ToContainTextAsync("1 clicks. Shift: false Ctrl: false Alt: false Meta: false");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Control },
        });
        await Expect(Page.Locator("#status")).ToContainTextAsync("2 clicks. Shift: false Ctrl: true Alt: false Meta: false");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Alt },
        });
        await Expect(Page.Locator("#status")).ToContainTextAsync("3 clicks. Shift: false Ctrl: false Alt: true Meta: false");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Shift },
        });
        await Expect(Page.Locator("#status")).ToContainTextAsync("4 clicks. Shift: true Ctrl: false Alt: false Meta: false");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Meta },
        });
        await Expect(Page.Locator("#status")).ToContainTextAsync("5 clicks. Shift: false Ctrl: false Alt: false Meta: true");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Control, KeyboardModifier.Shift },
        });
        await Expect(Page.Locator("#status")).ToContainTextAsync("6 clicks. Shift: true Ctrl: true Alt: false Meta: false");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Alt, KeyboardModifier.Shift },
        });
        await Expect(Page.Locator("#status")).ToContainTextAsync("7 clicks. Shift: true Ctrl: false Alt: true Meta: false");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Alt, KeyboardModifier.Control },
        });
        await Expect(Page.Locator("#status")).ToContainTextAsync("8 clicks. Shift: false Ctrl: true Alt: true Meta: false");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Alt, KeyboardModifier.Control, KeyboardModifier.Shift },
        });
        await Expect(Page.Locator("#status")).ToContainTextAsync("9 clicks. Shift: true Ctrl: true Alt: true Meta: false");
    }
}
