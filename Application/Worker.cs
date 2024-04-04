using Infrastructure.Interfaces;
using Infrastructure;
using Microsoft.Extensions.Hosting;
using Persistence;
using Domain;

namespace Application
{
    public class Worker : BackgroundService
    {
        private readonly int _qtdThreads = 1;
        private string[] _args;


        public Worker(string[] args)
        {
            _args = args;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queue = new Queue<string>(_args);

            while (!stoppingToken.IsCancellationRequested)
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
                                ISeleniumService _seleniumService = new AluraService();
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
