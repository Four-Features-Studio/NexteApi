using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NexteAPI.Commands;
using NexteAPI.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NexteAPI.BackgroundServices
{
    public class MainService : BackgroundService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IMediator _mediator;
        public MainService(ILogger<MainService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConsoleReader.ConsoleBlocker.Set();
            var sync_profiles = _mediator.Send(new SyncProfilesCommand());
            var sync_updates = _mediator.Send(new SyncUpdatesCommand());
            await Task.WhenAll(sync_profiles, sync_updates);
        }
    }
}
