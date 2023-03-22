using MediatR;
using NexteAPI.Commands;
using NexteAPI.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NexteAPI.CommandsHandler
{
    public class SyncUpdatesHandler : IRequestHandler<SyncUpdatesCommand>
    {
        IFileService _fileService;
        public SyncUpdatesHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task Handle(SyncUpdatesCommand request, CancellationToken cancellationToken)
        {
            await _fileService.SyncUpdates();
        }
    }
}

