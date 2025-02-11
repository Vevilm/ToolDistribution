using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.RepositoryInterfaces;
using ToolDistribution.SearchData;

namespace ToolDistribution.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        IArticleRepository repository;
        UserManager<User> userManager;

        public ArticlesController(IArticleRepository repository, UserManager<User> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string searchDataStr = "")
        {
            ArticleSearchData searchData = ArticleSearchData.GetSearchData(searchDataStr);
            int toolstotal = 0;
            searchData.statistics = await repository.GetStatistics();
            int totalCount = 0;
            var articles = repository.GetSearchPage(out totalCount, searchData.Value, page);
            searchData.Articles = articles;
            searchData.elementsCount = totalCount;
            return View(searchData);
        }

        [HttpPost]
        public IActionResult Index(ArticleSearchData searchData)
        {
            int totalCount = 0;
            var articles = repository.GetSearchPage(out totalCount, searchData.Value);
            searchData.Articles = articles;
            searchData.elementsCount = totalCount;
            return View(searchData);
        }

        #endregion Index

        #region Create

        [HttpGet]
        [Authorize(Roles = "Администратор, Кладовщик")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Администратор, Кладовщик")]
        [HttpPost]
        public IActionResult Create(Article article)
        {
            if (!ModelState.IsValid)
            {
                return View(article);
            }
            repository.Insert(article);
            return RedirectToAction("Index");
        }

        #endregion Create

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Article articleLocal = await repository.GetByIDAsync(id);
            int totalCount;
            var ToolsCount = repository.GetToolsCountPage(id, out totalCount);
            ArticlesDetailsData articlesDetailsData = new ArticlesDetailsData()
            {
                article = articleLocal,
                toolsCount = ToolsCount,
                elementsCount = totalCount,
                articleID = articleLocal.ID
            };
            return View(articlesDetailsData);
        }
        [HttpPost]
        public async Task<IActionResult> Details(string id, string detailsData)
        {
            Article articleLocal = await repository.GetByIDAsync(id);
            int count;
            var ToolsCount = repository.GetToolsCountPage(id, out count);
            ArticlesDetailsData articlesDetailsData = ArticlesDetailsData.GetSearchData(detailsData);
            articlesDetailsData.toolsCount = repository.GetToolsCountPage(id, out count,
                articlesDetailsData.querry, articlesDetailsData.PageNumber);
            return View(articlesDetailsData);
        }

        #endregion Details

        #region Edit

        [Authorize(Roles = "Администратор, Кладовщик")]
        [HttpGet]
        public async Task<IActionResult> Edit(string ID)
        {
            var article = await repository.GetByIDAsync(ID);
            return View(article);
        }

        [Authorize(Roles = "Администратор, Кладовщик")]
        [HttpPost]
        public IActionResult Edit(Article article)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(article);
                }
                repository.Update(article);
                return RedirectToAction("Details", new { article.ID });
            }
            catch (Exception)
            {

            }
            return View(article);
        }

        #endregion Edit

        [Authorize(Roles = "Администратор, Рабочий")]
        [HttpGet]
        public async Task<IActionResult> Request(string ID)
        {
            var article1 = await repository.GetByIDAsync(ID);
            int count;
            var toolsCount = repository.GetToolsCountPage(ID, out count);
            var ids = toolsCount
                .Where(tc => tc.avaliable > 0)
                .Select(s => s.StorageID);
            var storages = repository.GetStorages(ids);
            RequestData requestData = new RequestData()
            {
                article = article1,
                articleID = article1.ID,
                storages = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>(),
                userID = userManager.GetUserId(User)
            };
            foreach (var storage in storages)
            {
                requestData.storages.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(storage.Name, storage.ID.ToString()));
            }
            return View(requestData);
        }
        [Authorize(Roles = "Администратор, Рабочий")]
        [HttpPost]
        public async Task<IActionResult> Request(string storageID, string data)
        { 
            RequestData requestData = RequestData.Decode(data);
            var tool = repository.GetTool(requestData.articleID, Convert.ToInt16(storageID));
            var request = new ToolRequest()
            {
                WorkerID = requestData.userID.Replace(";", ""),
                ToolID = tool.ID,
                Requested = DateTime.Now,
                Status = Enums.ToolRequestStatus.Запрошен.ToString()
            };
            repository.InsertToolRequest(request);
            return RedirectToAction("Index");
        }
    }
}
