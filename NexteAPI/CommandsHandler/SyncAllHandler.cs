using MediatR;
using NexteAPI.Commands;
using NexteAPI.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NexteAPI.CommandsHandler
{
    public class SyncAllHandler : IRequestHandler<SyncAllCommand>
    {
        IFileService _fileService;
        public SyncAllHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task Handle(SyncAllCommand request, CancellationToken cancellationToken)
        {
            await _fileService.SyncAll();
        }
    }
}

