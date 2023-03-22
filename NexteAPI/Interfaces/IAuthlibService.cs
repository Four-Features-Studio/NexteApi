using System.Threading.Tasks;

namespace NexteAPI.Interfaces
{
    public interface IAuthlibService
    {
        Task<string> GetSignature(string data);
        Task<string> GetPublicKey();
    }
}
