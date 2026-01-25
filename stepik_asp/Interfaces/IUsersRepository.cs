using stepik_asp.Areas.AdminPanel.Models;
using stepik_asp.Models;

namespace stepik_asp.Interfaces
{
    public interface IUsersRepository
    {
        void Add(UserViewModel user);
        UserViewModel? TryGetByLogin(string login);
        List<UserViewModel> GetAll();
        UserViewModel? TryGetById(Guid userId);
        void Delete(Guid userId);
        void Update(UserViewModel user);
        void ChangePassword(string login, string newPassword);
        void ChangeRole(string login, Role? newRole);
    }
}