using Domain;
using Infrastructure.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
        private WebDriver? _driver;

        
        public void GetSelenium(string url)
        {
            _url = url;

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-notifications");

            _driver = new ChromeDriver(options);
        }

        public void DoWork(string[] args)
        {
            foreach (var item in args)
            {
                ItemInput itemInput = new (); 
                this.NavigateToInitialPage();
                _driver.
            }
            
        }

        public void FinishWork()
        {

        }

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
                //throw new Exception(msg);
            }
        }

    }
}
