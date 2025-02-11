using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToolDistribution.Models;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;
using ToolDistribution.RepositoryInterfaces;

namespace ToolDistribution.Repositories
{
    public class ToolRequestRepository : IToolRequestRepository
    {
        ApplicationDbContext _context;

        public ToolRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ToolRequest> GetByIDAsync(int id)
        {
            var toolRequest = await _context.ToolRequests.AsNoTracking().FirstAsync(tr => tr.ID == id);
            return toolRequest;
        }

        public IEnumerable<ToolRequestsView> GetSearchPage(out int totalCount, string status = "", string querry = "", int page = 1)
        {
            IQueryable<ToolRequestsView> toolRequestsViews = _context.toolRequestsView;

            if (!querry.IsNullOrEmpty())
            {
                toolRequestsViews = toolRequestsViews.Where(trw =>
                    trw.UserName.Contains(querry) || trw.ToolName.Contains(querry));
            }

            if (!status.IsNullOrEmpty())
            {
                toolRequestsViews = toolRequestsViews.Where(trw =>
                    trw.RequestStatus == status);
            }

            totalCount = toolRequestsViews.Count();

            if (totalCount > SearchData.SearchData.PageSize * (page - 1))
            {
                toolRequestsViews = toolRequestsViews.Skip(SearchData.SearchData.PageSize * (page - 1))
                    .Take(SearchData.SearchData.PageSize);
            }
            else if (page != 1)
            {
                toolRequestsViews = toolRequestsViews.Take(SearchData.SearchData.PageSize);
            }

            return toolRequestsViews.ToList();
        }

        public async Task<ToolRequestsView> GetViewByIDAsync(int id)
        {
            var toolRequestView = await _context.toolRequestsView.AsNoTracking().FirstAsync(tr => tr.RequestID == id);
            return toolRequestView;
        }

        public bool Insert(ToolRequest toolRequest)
        {
            _context.ToolRequests.Add(toolRequest);
            return Save();
        }

        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }

        public bool Update(ToolRequest toolRequest)
        {
            _context.Update(toolRequest);
            return Save();
        }
    }
}
