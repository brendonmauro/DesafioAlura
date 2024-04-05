using Infrastructure.Interfaces;
using Infrastructure;
using Microsoft.Extensions.Hosting;
using Persistence;
using Domain;

namespace Application
{   
    /// <summary>
    /// Classe Worker da aplicação
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly int _qtdThreads = 2;
        private ISeleniumService _seleniumService;
        private string[] _args;


        public Worker(ISeleniumService seleniumService, string[] args)
        {
            _seleniumService = seleniumService;
            _args = args;
        }

        /// <summary>
        /// Método onde ocorrar o fluxo da aplicação
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queue = new Queue<string>(_args);

            if (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    
                    while (queue.Count > 0)
                    {
                        var qtdTasks = queue.Count < _qtdThreads ? queue.Count : _qtdThreads;
                        Task[] tasks = new Task[qtdTasks];
                        for (int i = 0; i < qtdTasks; i++)
                        {
                            var term = queue.Dequeue();
                            tasks[i] = Task.Factory.StartNew(() =>
                            {
                                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                                _seleniumService.DoWork(i, term);
                            });
                        }


                        Task.WaitAll(tasks);
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                
            }
        }
    }
}
