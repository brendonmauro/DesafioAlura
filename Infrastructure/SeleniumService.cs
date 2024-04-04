using Domain;
using Domain.Interfaces;
using Infrastructure.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public abstract class SeleniumService : ISeleniumService
    {
        private string _url;
        public WebDriver _driver;

        public void GetSelenium(string url)
        {
            _url = url;

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-notifications");

            _driver = new ChromeDriver(options);
        }

        public void DoWork(string[] args)
        {
            try
            {
                this.GetSelenium(_url);
                foreach (var item in args)
                {
                    IItemInput itemInput = this.CreateInput(item);
                    this.NavigateToInitialPage();
                    IEnumerable<IItemResult> itemsResult = this.GettingData(itemInput);
                    this.MakePersistence(itemsResult);
                }
                
            } catch (Exception ex)
            {
                SaveLog(ex.Message);
            } finally
            {
                FinishWork();

            }
        }

        protected abstract IEnumerable<IItemResult> GettingData(IItemInput itemInput);
        protected abstract IItemInput CreateInput(string item);

        public void FinishWork()
        {
            _driver.Close();
        }
        public abstract void MakePersistence(IEnumerable<IItemResult> itemsResult);

        private void NavigateToInitialPage()
        {
            try
            {
                if (!_driver.Url.Equals(_url))
                    _driver.Navigate().GoToUrl(_url);
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
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

    }
}
