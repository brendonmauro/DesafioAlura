using Domain;
using Domain.Interfaces;
using Infrastructure.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Persistence;

namespace Infrastructure
{
    public abstract class SeleniumService : ISeleniumService
    {
        public string _url;

        public void DoWork(int tx, string item)
        {
            IWebDriver driver = this.InicializarDriver();
            try
            {
                IItemInput itemInput = this.CreateInput(item, driver);
                this.NavigateToInitialPage(driver);
                IEnumerable<IItemResult> itemsResult = this.GettingData(itemInput, driver);
                this.MakePersistence(itemsResult, driver);
                
            } catch (Exception ex)
            {
                SaveLog($"Thread {tx} método DoWork: " + ex.Message);
            } finally
            {
                FinishWork(driver);
            }
        }

        private IWebDriver InicializarDriver()
        {
            ChromeOptions options = new()
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };

            options.AddArgument("no-sandbox");
            options.AddArgument("headless");
            options.AddArgument("--profile-directory=Default");
            options.AddArgument("--disable-web-security");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--start-maximized");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--ignore-ssl-error");

            options.AddExcludedArgument("enable-logging");

            return new ChromeDriver(options);
        }

        protected abstract IEnumerable<IItemResult> GettingData(IItemInput itemInput, IWebDriver driver);
        protected abstract IItemInput CreateInput(string item, IWebDriver driver);

        public void FinishWork(IWebDriver driver)
        {
            driver.Close();
        }
        public abstract void MakePersistence(IEnumerable<IItemResult> itemsResult, IWebDriver driver);

        private void NavigateToInitialPage(IWebDriver driver)
        {
            try
            {
                if (!driver.Url.Equals(_url))
                    driver.Navigate().GoToUrl(_url);
            }
            catch (Exception e)
            {
                var msg = "Site não responde no momento, continuaremos tentando, exceção: " + e.Message;
                throw new Exception(msg);
            }
        }

        private void SaveLog(string message)
        {
            try
            {
                LogPersistence logPersistence = new();
                logPersistence.Insert(new Log(message));
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
