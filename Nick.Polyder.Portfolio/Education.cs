using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using static System.Net.Mime.MediaTypeNames;

namespace Nick.Polyder.Portfolio
{
    public class Education : PageTest
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

        public async Task Verify_Education_Toggle()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Education" }).ClickAsync();
            var educContex = page.GetByText("Computer Science and Engineering TE (Technological Education) – Computer Science & Telecommunications.");
            await Expect(educContex).ToBeVisibleAsync();

            var expaArrow = page.Locator("//*[@id=\"app\"]/div[3]/div/div/div[5]/div/div[4]/div[1]");
            await expaArrow.ClickAsync();
            await Expect(educContex).Not.ToBeVisibleAsync();
            await page.WaitForTimeoutAsync(3000);

        }


        [Test]
        public async Task Verify_Education_Contex()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Education" }).ClickAsync();
            var educContex = page.GetByText("Computer Science and Engineering TE (Technological Education) – Computer Science & Telecommunications.");
            await Expect(educContex).ToContainTextAsync("Computer Science and Engineering", new() { IgnoreCase = true });
        }

        [Test]

        public async Task Verify_Num_Of_Bachelor()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var bachCount = page.Locator("hr.bg-education-bachelor");

            await Expect(bachCount).ToHaveCountAsync(1);

        }


        [Test]

        public async Task Verify_Num_Of_Certs()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var bachCount = page.Locator("hr.bg-education-certificate");
            await Expect(bachCount).ToHaveCountAsync(5);

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

