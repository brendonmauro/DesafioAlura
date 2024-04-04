namespace Infrastructure.Interfaces
{
    public interface ISeleniumService
    {
        void GetSelenium(string url);
        void DoWork(string[] args);
    }
}
