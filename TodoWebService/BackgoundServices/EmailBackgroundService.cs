using System.Net.Mail;
using System.Net;
using TodoWebService.Data;
using TodoWebService.Models.Entities;
using TodoWebService.Services;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MimeKit;

namespace TodoWebService.BackgoundServices
{
    public class EmailBackgroundService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private TodoService? _todoService;
        private TodoDbContext? _todoDbContext;
        private IServiceProvider _provider;

        public EmailBackgroundService(IServiceProvider provider)
        {
            _provider = provider;
        }

        private void DoWork(object? state)
        {

            using var scope = _provider.CreateScope();
            _todoService = scope.ServiceProvider.GetService<ITodoService>() as TodoService;
            _todoDbContext = scope.ServiceProvider.GetService<TodoDbContext>()!;

            if (_todoService != null && _todoDbContext != null)
            {

                List<TodoItem> todo = _todoService!.GetTodoAllItems().Result;

                if (todo != null)
                {
                    foreach (var i in todo)
                        if (!i.Notify)
                        {
                            if (7 <= (DateTime.Now - i.CreatedTime).Days)
                            {
                                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                                {
                                    Port = 587,
                                    Credentials = new NetworkCredential("royalb9514@gmail.com", "lzen bgxq jagj nkkg"),
                                    EnableSsl = true,
                                };

                                smtpClient.Send("royalb9514@gmail.com", $"mirtalibhuseyinzade@gmail.com", "ToDo", $"Ala lom bax da bu todo-ya create {i.CreatedTime} elemisen. Bu zibilin vaxtin da, day demirem ozun tap:)");

                                _todoService.ChangeTodoNotify(i.Id, true).Wait();

                                if (i == todo.Last())
                                    break;
                            }
                        }
                }
            }
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
