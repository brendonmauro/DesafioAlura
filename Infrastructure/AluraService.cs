using Domain;
using Domain.Interfaces;
using OpenQA.Selenium;
using Persistence;
using System.Text.RegularExpressions;

namespace Infrastructure
{
    public class AluraService : SeleniumService
    {

        public AluraService() {
            this.GetSelenium("https://www.alura.com.br/");
        }

        public override void MakePersistence(IEnumerable<IItemResult> itemsResult)
        {
            ItemResultPersistence itemResultPersistence = new();
            itemsResult.ToList().ForEach(item =>  itemResultPersistence.Insert((ItemResult)item));
        }

        protected override IItemInput CreateInput(string item)
        {
            IItemInput itemInput = new ItemInput(item);

            return itemInput;
        }

        protected override IEnumerable<IItemResult> GettingData(IItemInput itemInput)
        {
            SearchResults(itemInput);
            var cards = GetCardsInformation();

            var items = new List<IItemResult>();
            foreach (var card in cards)
            {
                items.Add(CatchResult(card));
            }
            return items;
        }

        private void SearchResults(IItemInput itemInput)
        {
            var inputField = _driver.FindElement(By.Id("header-barraBusca-form-campoBusca"));
            inputField.SendKeys(itemInput.TextInput);
            var buttonSearch = _driver.FindElement(By.ClassName("header__nav--busca-submit"));
            buttonSearch.Submit();

            var buttonShowOptions = _driver.FindElement(By.ClassName("show-filter-options"));
            buttonShowOptions.Click();

            var buttonCurso = _driver.FindElement(By.CssSelector("ul[id*=busca--filtros--tipos] li"));
            buttonCurso.Click();

            var buttonFiltrar = _driver.FindElement(By.Id("busca--filtrar-resultados"));
            buttonFiltrar.Click();
        }

        private IEnumerable<ItemCard> GetCardsInformation()
        {
            var cards = _driver.FindElements(By.ClassName("busca-resultado"));

            return cards.Select(card => new ItemCard
            {
                Link = card.FindElement(By.ClassName("busca-resultado-link"))?.GetAttribute("href") ?? string.Empty,
                Titulo = card.FindElement(By.ClassName("busca-resultado-nome"))?.Text ?? string.Empty,
                Descricao = card.FindElement(By.ClassName("busca-resultado-descricao"))?.Text ?? string.Empty
            });
        }

        private IItemResult CatchResult(ItemCard card)
        {
            _driver.Navigate().GoToUrl(card.Link);

            var cargaHorariaText = _driver.FindElement(By.ClassName("course-card-wrapper-infos")).Text ?? "0";

            IItemResult itemResult = new ItemResult
            {
                Titulo = card.Titulo,
                Professor = _driver.FindElement(By.ClassName("instructor-title--name"))?.Text ?? string.Empty,
                CargaHoraria = Convert.ToInt32(Regex.Replace(cargaHorariaText, "[^0-9]", "")),
                Descricao = card.Descricao
            };
            _driver.Navigate().Back();


            return itemResult;
        }
    }
}
