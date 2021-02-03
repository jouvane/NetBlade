using System.Threading.Tasks;

namespace NetBlade.API.Security.Authorizations
{
    public interface IAuthorizationProvider
    {
        Task<string[]> GetRoles();

        Task<string> GetToken(string identificadorAutenticacao);
    }
}
