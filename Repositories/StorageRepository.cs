using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToolDistribution.Models;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.RepositoryInterfaces;

namespace ToolDistribution.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        ApplicationDbContext _context;

        public StorageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Storage> GetByIDAsync(int id)
        {
            var storage = await _context.Storages.AsNoTracking().FirstAsync(s => s.ID == id);
            return storage;
        }
        
        public IEnumerable<Storage> GetSearchPage(out int totalCount,
            Dictionary<string, string> filters = null, string querry = "", int page = 1)
        {
            IQueryable<Storage> storages = _context.Storages;
            if (!querry.IsNullOrEmpty())
            {
                storages = _context.Storages.AsNoTracking().Where(a => a.Name.Contains(querry));
            }
            if (!filters.IsNullOrEmpty())
            {
                storages = Filter(filters, storages);
            }
            totalCount = storages.Count();
            if (storages.Count() > SearchData.SearchData.PageSize * (page - 1))
            {
                storages = storages.Skip(SearchData.SearchData.PageSize * (page - 1))
                    .Take(SearchData.SearchData.PageSize);
            }
            else if (page != 1)
            {
                storages = storages.Take(SearchData.SearchData.PageSize);
            }
            return storages.ToList();
        }

        private IQueryable<Storage> Filter(Dictionary<string, string> filters,
            IQueryable<Storage> storages) {
            foreach (string key in filters.Keys)
            {
                switch (key.ToLower())
                {
                    case "type":
                        storages = storages.Where(s => s.Type == filters[key]);
                        break;
                    case "status":
                        storages = storages.Where(s => s.Status == filters[key]);
                        break;
                    default:
                        break;
                }
            }
            return storages;
        }

        public bool Insert(Storage storage)
        {
            _context.Storages.Add(storage);
            return Save();
        }

        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }

        public bool Update(Storage storage)
        {
            _context.Update(storage);
            return Save();
        }
    }
}
