using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.RepositoryInterfaces
{
    public interface IUserRepository
    {
        bool Insert(User user, string role);

        bool Update(User user, string role);

        bool Save();

        Task<User> GetByIDAsync(string id);

        IEnumerable<User> GetSearchPage(out int totalCount,
            Dictionary<string, string> filters = null,
            string querry = "", int page = 1);

        string GetRole(User user);

        IEnumerable<ToolRequestsView> GetToolsForUser(out int totalCount,
            string userid, string querry = "", int page = 1);
    }
}
