using BackBlaze.Client.IService.Models.Response;

namespace BackBlaze.Client.IService
{
    public interface IBackblazeAuthService
    {
        Task<BackblazeAuthState> GetAuthAsync();
    }
}
