using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.RepositoryInterfaces
{
    public interface IToolRepository
    {
        bool Insert(Tool storage);

        bool Update(Tool storage);

        bool Save();

        Task<ToolsView> GetViewByIDAsync(int id);

        Task<Tool> GetByIDAsync(int id);

        IEnumerable<ToolsView> GetSearchPage(out int totalCount, string searchMode = "all",
            string status = "", string querry = "", int page = 1);

    }
}
