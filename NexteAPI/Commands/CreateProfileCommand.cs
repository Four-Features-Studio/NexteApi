using MediatR;

namespace NexteAPI.Commands
{
    public class CreateProfileCommand : IRequest
    {
        public string Name;
        public string Version;
    }
}
