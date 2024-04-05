using Domain;
using Domain.Interfaces;
using Infrastructure.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Persistence;

namespace Infrastructure
{
    /// <summary>
    /// Classe modelo para um serviço que utilizará Selenium para coleta de dados
    /// </summary>
    public abstract class SeleniumService : ISeleniumService
    {
        public string _url;

        /// <summary>
        /// Método onde contém o fluxo principal do serviço
        /// </summary>
        /// <param name="tx">identificador do número da Thread usada</param>
        /// <param name="item">parâmetro de entrada de dados</param>
        public void DoWork(int tx, string item)
        {
            IWebDriver driver = this.InitializeDriver();
            try
            {
                IItemInput itemInput = this.CreateInput(item, driver);
                this.NavigateToInitialPage(driver);
                IEnumerable<IItemResult> itemsResult = this.GettingData(itemInput, driver);
                this.MakePersistence(itemsResult, driver);

            }
            catch (Exception ex)
            {
                SaveLog($"Thread {tx} método DoWork: " + ex.Message);
            }
            finally
            {
                FinishWork(driver);
            }
        }

        /// <summary>
        /// Método responsável pela inialização do chromedriver com as suas opções
        /// </summary>
        /// <returns>Retorna o WebDriver</returns>
        private IWebDriver InitializeDriver()
        {
            try
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
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Método responsável por pegar os dados no site a ser explorado
        /// </summary>
        /// <param name="itemInput">Objeto com a entrada de dados</param>
        /// <param name="driver">WebDriver sendo usado no service</param>
        /// <returns>Retorno a lista de objetos obtidos no site</returns>
        protected abstract IEnumerable<IItemResult> GettingData(IItemInput itemInput, IWebDriver driver);

        /// <summary>
        /// Método responsável por criar o objeto de entrada de dados
        /// </summary>
        /// <param name="item">string que contém a informação da entrada de dados</param>
        /// <param name="driver">WebDriver sendo usado no service</param>
        /// <returns>Retorna o objeto da entrada de dados</returns>
        protected abstract IItemInput CreateInput(string item, IWebDriver driver);


        /// <summary>
        /// Método por encerrar o que for necessário para finalizar o serviço, como por exemplo o Webdrivar
        /// </summary>
        /// <param name="driver">WebDriver sendo usado no service</param>
        public void FinishWork(IWebDriver driver)
        {
            driver.Close();
        }

        /// <summary>
        /// Método responsável por fazer a persistência dos objetos obtidos
        /// </summary>
        /// <param name="itemsResult">Objetos que serão persistidos</param>
        /// <param name="driver">WebDriver sendo usado no service</param>
        public abstract void MakePersistence(IEnumerable<IItemResult> itemsResult, IWebDriver driver);

        /// <summary>
        /// Método responsável por navegar para a Url Princial
        /// </summary>
        /// <param name="driver">WebDriver sendo usado no service</param>
        /// <exception cref="Exception">Exceção lançada quando o site não responde</exception>
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

        /// <summary>
        /// Método responsável por salvar os logs de exceções capturadas
        /// </summary>
        /// <param name="message">Mensagem da exceção</param>
        /// <exception cref="Exception">Exceção para quando falhar a comunicação com o banco de dados</exception>
        private void SaveLog(string message)
        {
            try
            {
                LogPersistence logPersistence = new();
                logPersistence.Insert(new Log(message));
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
