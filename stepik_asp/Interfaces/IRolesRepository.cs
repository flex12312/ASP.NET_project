using stepik_asp.Areas.AdminPanel.Models;

namespace stepik_asp.Interfaces
{
    public interface IRolesRepository
    {
        List<Role> GetAll();

        Role? TryGetByName(string roleName);

        Role? TryGetById(Guid roleId);

        void AddRole(Role role);

        void DeleteRole(Guid roleId);
    }

}
