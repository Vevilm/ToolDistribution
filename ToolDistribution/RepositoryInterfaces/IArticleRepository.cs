using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.RepositoryInterfaces
{
    public interface IArticleRepository
    {
        bool Insert(Article article);

        bool Update(Article article);

        bool Save();

        Task<Article> GetByIDAsync(string id);

        IEnumerable<Article> GetSearchPage(out int totalCount,
            string querry = "", int page = 1);

        IEnumerable<ToolsCount> GetToolsCountPage(string articleID,
            out int totalCount, string querry = "", int page = 1);

        IEnumerable<Storage> GetStorages(IEnumerable<short> Ids);

        Task<IEnumerable<ArticlesToToolsCount>> GetStatistics();

        bool InsertToolRequest(ToolRequest request);

        Tool GetTool(string ArticleID, short StorageID);
    }
}
