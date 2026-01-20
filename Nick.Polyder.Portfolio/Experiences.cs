using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using static System.Net.Mime.MediaTypeNames;

namespace Nick.Polyder.Portfolio
{
    public class Experiences : PageTest
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

        public async Task Verify_Experience_Toggle()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Experiences" }).ClickAsync();
            var firstJob = page.Locator("//*[@id=\"app\"]/div[3]/div/div/div[5]/div/div[3]/div[2]/div/div/div/div/div/div[1]/div[1]/div/div/div/div[1]/div/div[1]/h6");
            await Expect(firstJob).ToBeVisibleAsync();

            var expaArrow2 = page.Locator("//*[@id=\"app\"]/div[3]/div/div/div[5]/div/div[3]/div[1]");
            await expaArrow2.ClickAsync();
            await Expect(firstJob).Not.ToBeVisibleAsync();
            await page.WaitForTimeoutAsync(3000);

        }

        [Test]

        public async Task Verify_Experience_Stepper_Step1()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Experiences" }).ClickAsync();

            var step = "Enterprise Registry Solutions";
            await page.Locator($"button[aria-controls='{step}']").ClickAsync();
            var secondStar = page.Locator($"div[aria-labelledby='{step}']");
            await Expect(secondStar).ToContainTextAsync("As Application Architect for ERS", new() { IgnoreCase = true });



        }


        [Test]

        public async Task Verify_Experience_Stepper_Step2()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Experiences" }).ClickAsync();



            var step = "Customedialabs";
            await page.Locator($"button[aria-controls='{step}']").ClickAsync();
            var thirdStar = page.Locator($"div[aria-labelledby='{step}']");
            await Expect(thirdStar).ToContainTextAsync("LMS portal", new() { IgnoreCase = true });



        }


        [Test]

        public async Task Verify_Experience_Stepper_Step3()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            await page.GetByRole(AriaRole.Heading, new() { Name = "Experiences" }).ClickAsync();



            var step = "ITWorx";
            await page.Locator($"button[aria-controls='{step}']").ClickAsync();
            var fourthStar = page.Locator($"div[aria-labelledby='{step}']");
            await Expect(fourthStar).ToContainTextAsync("HIVICU", new() { IgnoreCase = true });

        }



        [Test]

        public async Task Verify_First_Experience_Link()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var waitForPageTask = page.Context.WaitForPageAsync();
            await page.GetByRole(AriaRole.Heading, new() { Name = "Experiences" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "https://www.microsoft.com" }).ClickAsync();
            string expectedUrl = "https://www.microsoft.com/en-us";
            Assert.That(page.Url, Is.EqualTo(expectedUrl));
            await page.GoBackAsync();
            expectedUrl = "https://nickpolyder.github.io/";
            Assert.That(page.Url.Equals(expectedUrl));
        }

        [Test]

        public async Task Verify_Second_Experience_Link()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var waitForPageTask = page.Context.WaitForPageAsync();
            await page.GetByRole(AriaRole.Heading, new() { Name = "Experiences" }).ClickAsync();
            var step = "Enterprise Registry Solutions";
            await page.Locator($"button[aria-controls='{step}']").ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "https://www.ersl.ie/" }).ClickAsync();
            string expectedUrl = "https://ersl.ie/";
            Assert.That(page.Url, Is.EqualTo(expectedUrl));
            await page.GoBackAsync();
            expectedUrl = "https://nickpolyder.github.io/";
            Assert.That(page.Url.Equals(expectedUrl));
        }



        [Test]

        public async Task Verify_Third_Experience_Link()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var waitForPageTask = page.Context.WaitForPageAsync();
            await page.GetByRole(AriaRole.Heading, new() { Name = "Experiences" }).ClickAsync();
            var step = "Customedialabs";
            await page.Locator($"button[aria-controls='{step}']").ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "https://customedialabs.com" }).ClickAsync();
            string expectedUrl = "https://www.customedialabs.com/";
            Assert.That(page.Url, Is.EqualTo(expectedUrl));
            await page.GoBackAsync();
            expectedUrl = "https://nickpolyder.github.io/";
            Assert.That(page.Url.Equals(expectedUrl));
        }


        [Test]

        public async Task Verify_Forth_Experience_Link()
        {
            await page.GotoAsync("https://nickpolyder.github.io/");
            var waitForPageTask = page.Context.WaitForPageAsync();
            await page.GetByRole(AriaRole.Heading, new() { Name = "Experiences" }).ClickAsync();
            var step = "ITWorx";
            await page.Locator($"button[aria-controls='{step}']").ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "https://itworx.gr" }).ClickAsync();
            string expectedUrl = "https://itworx.gr/";
            Assert.That(page.Url, Is.EqualTo(expectedUrl));
            await page.GoBackAsync();
            expectedUrl = "https://nickpolyder.github.io/";
            Assert.That(page.Url.Equals(expectedUrl));
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
