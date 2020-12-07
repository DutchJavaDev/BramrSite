using System.Threading.Tasks;

namespace BramrSite.Auth
{
    public interface ITokenHandler
    {
        Task UpdateAutenticationState(string token);

        Task<bool> HasToken();
    }
}
