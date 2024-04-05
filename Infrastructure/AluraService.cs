using Domain;
using Domain.Interfaces;
using OpenQA.Selenium;
using Persistence;
using System.Text.RegularExpressions;

namespace Infrastructure
{   
    /// <summary>
    /// Classe responsável por fazer as tarefas do desafio
    /// </summary>
    public class AluraService : SeleniumService
    {
        /// <summary>
        /// No construtor temos a url da alura
        /// </summary>
        public AluraService() {
            this._url = "https://www.alura.com.br/";
        }

        public override void MakePersistence(IEnumerable<IItemResult> itemsResult, IWebDriver driver)
        {
            ItemResultPersistence itemResultPersistence = new();
            itemsResult.ToList().ForEach(item =>  itemResultPersistence.Insert((ItemResult)item));
        }

        protected override IItemInput CreateInput(string item, IWebDriver driver)
        {
            IItemInput itemInput = new ItemInput(item);

            return itemInput;
        }

        protected override IEnumerable<IItemResult> GettingData(IItemInput itemInput, IWebDriver driver)
        {
            try
            {
                SearchResults(itemInput, driver);
                var cards = GetCardsInformation(driver);

                var items = new List<IItemResult>();
                foreach (var card in cards)
                {
                    items.Add(CatchResult(card, driver));
                }
                return items;
            } catch (Exception ex) { throw new Exception("Erro ao pegar as informacoes do curso: " + ex.Message); }
        }

        /// <summary>
        /// Método onde são realizadas todas as ações necessárias para pesquisar os resultados
        /// </summary>
        /// <param name="itemInput">Objeto com a entrada de dados</param>
        /// <param name="driver">WebDriver sendo usado no service</param>
        /// <exception cref="Exception">Exceção será lançada quando o webdriver por algum motivo não conseguir fazer a pesquisa</exception>
        private void SearchResults(IItemInput itemInput, IWebDriver driver)
        {
            try
            {
                var inputField = driver.FindElement(By.Id("header-barraBusca-form-campoBusca"));
                inputField.SendKeys(itemInput.TextInput);
                var buttonSearch = driver.FindElement(By.ClassName("header__nav--busca-submit"));
                buttonSearch.Submit();

                var buttonShowOptions = driver.FindElement(By.ClassName("show-filter-options"));
                buttonShowOptions.Click();

                var buttonCurso = driver.FindElement(By.CssSelector("ul[id*=busca--filtros--tipos] li"));
                buttonCurso.Click();

                var buttonFiltrar = driver.FindElement(By.Id("busca--filtrar-resultados"));
                buttonFiltrar.Click();
            } catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar o termo: " + ex.Message);
            }
        }

        /// <summary>
        /// Método onde obtém as informações nos cards
        /// </summary>
        /// <param name="driver">WebDriver sendo usado no service</param>
        /// <returns>Retorna objeto com as informações do card</returns>
        /// <exception cref="Exception">Exceção lançada quando não consegue obter a informação no card</exception>
        private IEnumerable<ItemCard> GetCardsInformation(IWebDriver driver)
        {
            try
            {
                var cards = driver.FindElements(By.ClassName("busca-resultado"));

                 var itemCards = cards.Select(card => new ItemCard
                {
                    Link = card.FindElement(By.ClassName("busca-resultado-link"))?.GetAttribute("href") ?? string.Empty,
                    Titulo = card.FindElement(By.ClassName("busca-resultado-nome"))?.Text ?? string.Empty,
                    Descricao = card.FindElement(By.ClassName("busca-resultado-descricao"))?.Text ?? string.Empty
                }).ToList();

                return itemCards;
            } catch (Exception ex) { throw new Exception("Erro ao pegar informações os cards dos resultados: " + ex.Message); }
        }

        /// <summary>
        /// Método onde obtém as informações sobre o professor e a carga horária
        /// </summary>
        /// <param name="card">Parâmetro com o objeto do card</param>
        /// <param name="driver">WebDriver sendo usado no service</param>
        /// <returns>Retorna o objeto completo com as informações completas</returns>
        /// <exception cref="Exception">Exceção lançada provavelmente quando tentar obter a informação de carga horária ou o nome do professor</exception>
        private IItemResult CatchResult(ItemCard card, IWebDriver driver)
        {
            try
            {
                driver.Navigate().GoToUrl(card.Link);

                var cargaHorariaText = driver.FindElement(By.ClassName("course-card-wrapper-infos")).Text ?? "0";

                IItemResult itemResult = new ItemResult
                {
                    Titulo = card.Titulo,
                    Professor = driver.FindElement(By.ClassName("instructor-title--name"))?.Text ?? string.Empty,
                    CargaHoraria = Convert.ToInt32(Regex.Replace(cargaHorariaText, "[^0-9]", "")),
                    Descricao = card.Descricao
                };
                driver.Navigate().Back();


                return itemResult;
            } catch (Exception ex) { throw new Exception("Erro ao passar as informacoes para o objeto: " + ex.Message); }
        }
    }
}
