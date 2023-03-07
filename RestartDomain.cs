using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

public class RestartDomain : IDisposable
{
    private readonly IWebDriver _driver;
    public RestartDomain() => _driver = new ChromeDriver();
    public void Dispose()
    {
        _driver.Close();
        _driver.Quit();
        _driver.Dispose();
        Environment.Exit(1);
    }

    public void InitiateDestructionSequence()
    {
        _driver.Navigate()
            .GoToUrl("http://192.168.10.6:888/Security/Update/RestartAppDomain");

        if (_driver.Title == "HRIS Login")
        {
            _driver.FindElement(By.Id("LoginID"))
                    .SendKeys("SUPER");


            _driver.FindElement(By.Id("LoginPassword"))
                .SendKeys("sup@nimble");

            _driver.FindElement(By.Id("btnSubmit"))
            .Click();
        }

    }
    public void FinalBlow()
    {
        _driver.Navigate()
            .GoToUrl("http://192.168.10.6:888/Security/Update/RestartAppDomain");

        _driver.FindElement(By.Id("submit"))
            .Click();

    }
}