using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

public class RestartDomain : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private string _url = "192.168.10.6:888";

    public RestartDomain(string url, bool isManual) : this()
    {
        _url = url;
        Console.WriteLine($"http://{url}/");
        if (isManual)
        {

        }
    }

    public RestartDomain()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.EnableVerboseLogging = false;
        service.SuppressInitialDiagnosticInformation = true;
        service.HideCommandPromptWindow = true;


        ChromeOptions options = new ChromeOptions();
        options.AddArguments("--incognito");
        options.PageLoadStrategy = PageLoadStrategy.Normal;

        options.AddArgument("--no-sandbox");
        options.AddArgument("--headless");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--disable-crash-reporter");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-in-process-stack-traces");
        options.AddArgument("--disable-logging");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--log-level=3");
        options.AddArgument("--output=/dev/null");
        _driver = new ChromeDriver(service, options);

        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(60));
    }

    public void Dispose()
    {
        _driver.Close();
        _driver.Quit();
        _driver.Dispose();
        Environment.Exit(1);
    }

    public void InitiateDestructionSequence()
    {
        try
        {
            _driver.Navigate()
                        .GoToUrl($"http://{_url}/Security/Update/RestartAppDomain");

            if (_driver.Title.Contains("Login"))
            {
                _driver.FindElement(By.Id("LoginID"))
                        .SendKeys("SUPER");


                _driver.FindElement(By.Id("LoginPassword"))
                    .SendKeys("sup@nimble");

                _driver.FindElement(By.Id("btnSubmit")).Click();
                Console.WriteLine("Logging into the system.\n");
            }


            Console.WriteLine("Waiting for restart page to load.\n");

            _wait.Until(ExpectedConditions.ElementExists(By.Id("submit")));
            Console.WriteLine("Restart page loaded.\n");

            _driver.FindElement(By.Id("submit")).Click();
        }
        catch
        {
            Console.WriteLine("Failed to restart the domain. Please do it manually.\nSorry :D\n");
        }



        _wait.Until(ExpectedConditions.ElementExists(By.Id("LoginID")));

        Console.WriteLine("Restarted succesfully.\n");
        this.ClosingSequence();

    }
    public void ClosingSequence()
    {
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
        this.Dispose();
    }
}