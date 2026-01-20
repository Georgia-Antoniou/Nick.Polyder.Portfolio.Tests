using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nick.Polyder.Portfolio
{
    public class  BlogTests : PageTest
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

        public async Task Verify_Blog_Url() 
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Link, new() { Name = "Blog" }).ClickAsync();
            Assert.That(page.Url, Is.EqualTo("https://nickpolyder.github.io/blog"));
        }

        [Test]

        public async Task Verify_Blog_Content()
        {
            await page.GotoAsync("https://nickpolyder.github.io/blog");
            var content = page.GetByRole(AriaRole.Heading, new() { Name = "Coming Soon" });
            string? actualText = await content.TextContentAsync();
            Assert.That(actualText, Is.EqualTo("Coming Soon"));
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

                TestContext.WriteLine($"Failure detected at URL: {Page.Url}");

                await page.ScreenshotAsync(new() { Path = fullPath, FullPage = true });
                await page.WaitForTimeoutAsync(4000);

                TestContext.AddTestAttachment(fullPath);
            }

            await page.CloseAsync();
        }

    }
}
