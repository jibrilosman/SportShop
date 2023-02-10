using OnlineSportShop.ViewModel;

namespace OnlineSportShop.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginVM model);
        Task<Status> RegistrationAsync(RegistrationVM model);
        Task  LogoutAsync();
    }
}
