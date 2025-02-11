using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;
using ToolDistribution.RepositoryInterfaces;
using ToolDistribution.SearchData;

namespace ToolDistribution.Controllers
{
    [Authorize(Roles = "Кладовщик, Администратор")]
    public class ToolsController : Controller
    {
        IToolRepository repository;

        public ToolsController(IToolRepository repository)
        {
            this.repository = repository;
        }

        #region Index

        [HttpGet]
        public IActionResult Index(int page = 1, string searchDataStr = "")
        {
            ToolSearchData searchData = ToolSearchData.GetSearchData(searchDataStr);
            int totalCount = 0;
            string status = searchData.Status == "Все" ? "" : searchData.Status;
            var tools = repository.GetSearchPage(out totalCount, "all",
                status, searchData.Value, page);
            searchData.Tools = tools;
            searchData.elementsCount = totalCount;
            return View(searchData);
        }

        [HttpPost]
        public IActionResult Index(ToolSearchData searchData)
        {
            int totalCount = 0;
            string status = searchData.Status == "Все" ? "" : searchData.Status;
            var tools = repository.GetSearchPage(out totalCount, "all",
                status, searchData.Value);
            searchData.Tools = tools;
            searchData.elementsCount = totalCount;
            return View(searchData);
        }

        #endregion Index

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tool tool)
        {
            if (!ModelState.IsValid)
            {
                return View(tool);
            }
            repository.Insert(tool);
            return RedirectToAction("Index");
        }

        #endregion Create

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ToolsView toolsView = await repository.GetViewByIDAsync(id);
            return View(toolsView);
        }

        #endregion Details

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int ID)
        {
            var tool = await repository.GetByIDAsync(ID);
            return View(tool);
        }

        [HttpPost]
        public IActionResult Edit(Tool tool)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(tool);
                }
                repository.Update(tool);
                return RedirectToAction("Details", new { tool.ID });
            }
            catch (Exception)
            {

            }
            return View(tool);
        }

        #endregion Edit
    }
}
