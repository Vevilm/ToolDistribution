using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToolDistribution.Models;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;
using ToolDistribution.RepositoryInterfaces;

namespace ToolDistribution.Repositories
{
    public class ToolRepository : IToolRepository
    {
        ApplicationDbContext _context;

        public ToolRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tool> GetByIDAsync(int id)
        {
            var tool = await _context.Tools.AsNoTracking().FirstAsync(s => s.ID == id);
            return tool;
        }

        public async Task<ToolsView> GetViewByIDAsync(int id)
        {
            var toolView = await _context.toolsView.AsNoTracking().FirstAsync(s => s.ID == id);
            return toolView;
        }

        public IEnumerable<ToolsView> GetSearchPage(out int totalCount, string searchMode = "",
            string status = "", string querry = "", int page = 1)
        {
            IQueryable<ToolsView> toolViews = _context.toolsView;

            if (!querry.IsNullOrEmpty())
            {
                switch (searchMode.ToLower())
                {
                    case "name":
                        toolViews = _context.toolsView.AsNoTracking().Where(t =>
                        t.Name.Contains(querry));
                        break;
                    case "storage":
                        toolViews = _context.toolsView.AsNoTracking().Where(t =>
                        t.Storage.Contains(querry));
                        break;
                    default:
                        toolViews = _context.toolsView.AsNoTracking().Where(t =>
                        t.Storage.Contains(querry) || t.Name.Contains(querry));
                        break;
                }
            }

            if (!status.IsNullOrEmpty())
            {
                toolViews = toolViews.Where(t => t.Status == status);
            }

            totalCount = toolViews.Count();
            
            if (totalCount > SearchData.SearchData.PageSize * (page - 1))
            {
                toolViews = toolViews.Skip(SearchData.SearchData.PageSize * (page - 1))
                    .Take(SearchData.SearchData.PageSize);
            }
            else if (page != 1)
            {
                toolViews = toolViews.Take(SearchData.SearchData.PageSize);
            }

            return toolViews.ToList();
        }

        public bool Insert(Tool tool)
        {
            _context.Tools.Add(tool);
            return Save();
        }

        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }

        public bool Update(Tool tool)
        {
            _context.Update(tool);
            return Save();
        }
    }
}
