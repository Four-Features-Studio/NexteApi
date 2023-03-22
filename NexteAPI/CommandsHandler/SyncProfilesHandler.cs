using MediatR;
using NexteAPI.Commands;
using NexteAPI.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NexteAPI.CommandsHandler
{
    public class SyncProfilesHandler : IRequestHandler<SyncProfilesCommand>
    {
        IFileService _fileService;
        public SyncProfilesHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task Handle(SyncProfilesCommand request, CancellationToken cancellationToken)
        {
            await _fileService.SyncProfiles();
        }
    }
}
