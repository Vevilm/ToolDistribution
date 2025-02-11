using ToolDistribution.Models.DBmodels;

namespace ToolDistribution.RepositoryInterfaces
{
    public interface IStorageRepository
    {
        bool Insert(Storage storage);

        bool Update(Storage storage);

        bool Save();

        Task<Storage> GetByIDAsync(int id);

        IEnumerable<Storage> GetSearchPage(out int totalCount,
            Dictionary<string, string> filters = null,
            string querry = "", int page = 1);
    }
}
