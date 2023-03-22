using MediatR;
using NexteAPI.Commands;
using NexteAPI.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NexteAPI.CommandsHandler
{
    public class CreateProfilesHandler : IRequestHandler<CreateProfileCommand>
    {
        IFileService _fileService;
        public CreateProfilesHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            await _fileService.CreateProfile(request);
        }
    }
}
