using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Nick.Polyder.Portfolio
{
    public class HomePageTests : PageTest
    {
        private IBrowser browser;
        private IPage page;

        [SetUp]
        public async Task Setup()
        {
            browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            page = await browser.NewPageAsync();
            await Context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true });
        }

        [Test]
        public async Task Verify_Url()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            Assert.That(page.Url, Is.EqualTo("https://nickpolyder.github.io/"));
        }


        [Test]

        public async Task Verify_Name()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var name = page.Locator("//*[@id=\"app\"]/div[3]/div/div/div[2]/h2");
            string? actualText = await name.TextContentAsync();
            Assert.That(actualText, Is.EqualTo("Nick Polyderopoulos"));
        }

        [Test]

        public async Task Verify_Profession()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var profession = page.Locator("//*[@id=\"app\"]/div[3]/div/div/div[3]/h5");
            string? actualText = await profession.TextContentAsync();
            Assert.That(actualText, Is.EqualTo("Software Engineer II @ Microsoft"));
        }

        [Test]
        public async Task Verify_Theme_Toggle_Swaps_CSS_File()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");

            var themeToggle = page.Locator("//*[@id=\"app\"]/div[3]/header/div/button[2]/span");

            await themeToggle.ClickAsync();

            var themeLink = page.Locator("#prism-theme");
            await Expect(themeLink).ToHaveAttributeAsync("href", new Regex("light"));

            await themeToggle.ClickAsync();
            await Expect(themeLink).ToHaveAttributeAsync("href", new Regex("dark"));
        }

        [Test]

        public async Task Verify_Hamburger_Icon_Toggles_Sidebar()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");

            await page.GetByRole(AriaRole.Button).First.ClickAsync();
            var sidebar = page.Locator("#nav-drawer");
            await Expect(sidebar).ToHaveClassAsync(new Regex(".+close.+"));
            await page.GetByRole(AriaRole.Button).First.ClickAsync();
            await Expect(sidebar).ToHaveClassAsync(new Regex(".+open.+"));
        }

        [Test]

        public async Task Verify_Git_Link()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var waitForPageTask = page.Context.WaitForPageAsync();
            await page.Locator("//*[@id=\"nav-drawer\"]/div/footer/div/div/div[2]/div/div[1]/a").ClickAsync();
            IPage TabPage = await waitForPageTask;
            await TabPage.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            string expectedUrl = "https://github.com/NickPolyder";
            Assert.That(TabPage.Url, Is.EqualTo(expectedUrl));
        }

        [Test]

        public async Task Verify_LinkedIn_Link()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var waitForPageTask = page.Context.WaitForPageAsync();
            await page.Locator("//*[@id=\"nav-drawer\"]/div/footer/div/div/div[2]/div/div[2]/a/span").ClickAsync();
            IPage TabPage = await waitForPageTask;
            await TabPage.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            string expectedUrl = "https://www.linkedin.com/in/nick-polyderopoulos-21742397/";
            Assert.That(TabPage.Url, Is.EqualTo(expectedUrl));
        }

        [Test]

        public async Task Verify_Twitter_Link()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var waitForPageTask = page.Context.WaitForPageAsync();
            await page.Locator("//*[@id=\"nav-drawer\"]/div/footer/div/div/div[2]/div/div[3]/a/span").ClickAsync();
            IPage TabPage = await waitForPageTask;
            await TabPage.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            string expectedUrl = "https://x.com/nickpolyder";
            Assert.That(TabPage.Url, Is.EqualTo(expectedUrl));
        }



        [TearDown]
        public async Task TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {

                await page.WaitForTimeoutAsync(4000);
               
                await page.EvaluateAsync($@"() => {{
                const div = document.createElement('div');
                div.innerHTML = 'URL: ' + window.location.href;
                div.style.cssText = 'position:fixed;top:0;left:0;background:red;color:white;z-index:9999;padding:10px;font-family:sans-serif;';
                document.body.appendChild(div);
                }}");

                string screenshotFolder = @"..\..\..\Screenshots";
                string fileName = $"{TestContext.CurrentContext.Test.Name}_Failed.png";
                string fullPath = Path.Combine(screenshotFolder, fileName);

                TestContext.WriteLine($"Failure detected at URL: {page.Url}");
                
                await page.ScreenshotAsync(new() { Path = fullPath, FullPage = true });
                await page.WaitForTimeoutAsync(4000);

                TestContext.AddTestAttachment(fullPath);
            }

            await page.CloseAsync();
        }
    }

}
