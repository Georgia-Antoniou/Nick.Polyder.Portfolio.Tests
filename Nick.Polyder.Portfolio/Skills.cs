using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using static System.Net.Mime.MediaTypeNames;

namespace Nick.Polyder.Portfolio
{
    public class Skills : PageTest
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

        public async Task Verify_Skills_Toggle()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Skills" }).ClickAsync();
            var educContex = page.GetByText("Frameworks");
            await Expect(educContex).ToBeVisibleAsync();

            var expaArrow = page.Locator("//*[@id=\"app\"]/div[3]/div/div/div[5]/div/div[2]/div[1]");
            await expaArrow.ClickAsync();
            await Expect(educContex).Not.ToBeVisibleAsync();

        }

        [Test]

        public async Task Verify_Familiar_Skill_Filtering()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Skills" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Familiar" }).ClickAsync();
            await Expect(page.GetByText("UWP", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByText("Methodologies")).ToBeHiddenAsync();
        }

        [Test]

        public async Task Verify_Intermediate_Skill_Filtering()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Skills" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Intermediate" }).ClickAsync();
            await Expect(page.GetByText("Xamarin", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByText("Design Patterns")).ToBeHiddenAsync();
        }

        [Test]

        public async Task Verify_Advanced_Skill_Filtering()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Skills" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Advanced" }).ClickAsync();
            await Expect(page.GetByText("ADO.NET", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByText("CI/CD Pipelines")).ToBeHiddenAsync();
        }

        [Test]

        public async Task Verify_Expert_Skill_Filtering()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Skills" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Expert" }).ClickAsync();
            await Expect(page.GetByText("MS SQL", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByText("MassTransit")).ToBeHiddenAsync();
        }

        [Test]

        public async Task Verify_ShowAll_Skill_Filtering()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Skills" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Show All" }).ClickAsync();
            await Expect(page.GetByText("MS SQL", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByText("UWP", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByText("ADO.NET", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByText("WPF", new() { Exact = true })).ToBeVisibleAsync();

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

