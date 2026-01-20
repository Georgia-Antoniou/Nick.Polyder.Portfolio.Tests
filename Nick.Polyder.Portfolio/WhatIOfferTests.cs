using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using static System.Net.Mime.MediaTypeNames;

namespace Nick.Polyder.Portfolio
{
    public class WhatIOfferTests : PageTest
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

        public async Task Verify_What_I_Offer_Toggle()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");

            await page.GetByRole(AriaRole.Heading, new() { Name = "What I Offer" }).ClickAsync();
            var whatIOffer = page.GetByText("Innovation-Driven Leadership", new() { Exact = false });

            await Expect(whatIOffer).ToBeVisibleAsync();

            var expaArrow = page.Locator("//*[@id=\"app\"]/div[3]/div/div/div[5]/div/div[1]/div[1]");
            await expaArrow.ClickAsync();

            await Expect(whatIOffer).Not.ToBeVisibleAsync();
            await page.WaitForTimeoutAsync(3000);
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
        }
    }

}
