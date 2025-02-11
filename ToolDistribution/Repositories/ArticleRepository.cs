using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.IdentityModel.Tokens;
using ToolDistribution.Models;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;
using ToolDistribution.RepositoryInterfaces;

namespace ToolDistribution.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Article> GetByIDAsync(string id)
        {
            var article = await _context.Articles.AsNoTracking()
                .FirstAsync(s => s.ID == id);
            return article;
        }

        public IEnumerable<Article> GetSearchPage(out int totalCount,
            string querry = "", int page = 1)
        {
            IQueryable<Article> articles = _context.Articles;
            if (!querry.IsNullOrEmpty())
            {
                articles = _context.Articles.Where(a =>
                a.ID.Contains(querry) || a.Name.Contains(querry));
            }
            totalCount = articles.Count();
            if (articles.Count() > SearchData.SearchData.PageSize * (page - 1))
            {
                articles = articles.Skip(SearchData.SearchData.PageSize * (page - 1))
                    .Take(SearchData.SearchData.PageSize);
            }
            else if (page != 1)
            {
                articles = articles.Take(SearchData.SearchData.PageSize);
            }
            return articles.ToList();
        }

        async Task<IEnumerable<ArticlesToToolsCount>> IArticleRepository.GetStatistics()
        {
            var result = _context.toolsCount.Select(selector =>
                new ArticlesToToolsCount()
                {
                    ArticleID = selector.ArticleID,
                    Avaliable = _context.toolsCount
                        .Where(sel => sel.ArticleID == selector.ArticleID)
                        .Sum(sel => sel.avaliable)
                }
            );
            return result;
        }

        public IEnumerable<Models.DBmodels.Storage> GetStorages(IEnumerable<short> Ids)
        {
            return _context.Storages.Where(s => Ids.Contains(s.ID));
        }

        public bool Insert(Article article)
        {
            _context.Articles.Add(article);
            return Save();
        }

        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }

        public bool Update(Article article)
        {
            _context.Update(article);
            return Save();
        }

        IEnumerable<ToolsCount> IArticleRepository.GetToolsCountPage(string articleID,
            out int totalCount, string querry = "", int page = 1)
        {
            var result = _context.toolsCount.Where(tc
                => tc.ArticleID == articleID && tc.Storage.Contains(querry));
            totalCount = result.Count();
            //if (result.Count() > SearchData.SearchData.PageSize * (page - 1))
            //{
            //    result = result.Skip(SearchData.SearchData.PageSize * (page - 1))
            //        .Take(SearchData.SearchData.PageSize);
            //}
            //else if (page != 1)
            //{
            //    result = result.Take(SearchData.SearchData.PageSize);
            //}
            return result.ToList();
        }

        public bool InsertToolRequest(ToolRequest request)
        {
            _context.ToolRequests.Add(request);
            return Save();
        }

        public Tool GetTool(string ArticleID, short StorageID)
        {
            var result = _context.Tools.FirstOrDefault(t => t.ArticleID == ArticleID
                && t.StorageID == StorageID && t.Status == "Доступен");
            result.Status = "Занят";
            return result;
        }
    }
}
