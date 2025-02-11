using ToolDistribution.Models.DBmodels.Views;
using ToolDistribution.Models.DBmodels;

namespace ToolDistribution.RepositoryInterfaces
{
    public interface IToolRequestRepository
    {
        bool Insert(ToolRequest toolRequest);

        bool Update(ToolRequest toolRequest);

        bool Save();

        Task<ToolRequestsView> GetViewByIDAsync(int id);

        Task<ToolRequest> GetByIDAsync(int id);

        IEnumerable<ToolRequestsView> GetSearchPage(out int totalCount,
            string status = "", string querry = "", int page = 1);
    }
}
