using Infrastructure;
using Infrastructure.Interfaces;

namespace Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ISeleniumService _seleniumService = new AluraService();
                _seleniumService.DoWork(args);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
