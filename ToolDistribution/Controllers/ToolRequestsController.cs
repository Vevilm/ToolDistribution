using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.RepositoryInterfaces;
using ToolDistribution.SearchData;

namespace ToolDistribution.Controllers
{
    [Authorize]
    public class ToolRequestsController : Controller
    {
        IToolRequestRepository repository;

        public ToolRequestsController(IToolRequestRepository repository)
        {
            this.repository = repository;
        }

        #region Index

        [HttpGet]
        [Authorize(Roles = "Администратор, Кладовщик")]
        public IActionResult Index(int page = 1, string searchDataStr = "")
        {
            ToolRequestSearchData searchData = ToolRequestSearchData.GetSearchData(searchDataStr);
            int totalCount = 0;
            string status = "";
            if (searchData.Status != "Все") status = searchData.Status;
            var toolRequests = repository.GetSearchPage(out totalCount, status, searchData.Value, page);
            searchData.ToolRequests = toolRequests;
            searchData.elementsCount = totalCount;
            return View(searchData);
        }

        [Authorize(Roles = "Администратор, Кладовщик")]
        [HttpPost]
        public IActionResult Index(ToolRequestSearchData searchData)
        {
            int totalCount = 0;
            string status = "";
            if (searchData.Status != "Все") status = searchData.Status;
            var toolRequests = repository.GetSearchPage(out totalCount, status, searchData.Value, searchData.PageNumber);
            searchData.ToolRequests = toolRequests;
            searchData.elementsCount = totalCount;
            return View(searchData);
        }

        #endregion Index

        #region Create

        [Authorize(Roles = "Администратор, Кладовщик")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Администратор, Кладовщик")]
        [HttpPost]
        public IActionResult Create(ToolRequest toolRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(toolRequest);
            }
            repository.Insert(toolRequest);
            return RedirectToAction("Index");
        }

        #endregion Create

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var toolRequestView = await repository.GetViewByIDAsync(id);
            return View(toolRequestView);
        }

        #endregion Details

        #region Edit

        [HttpGet]
        [Authorize(Roles = "Администратор, Кладовщик")]
        public async Task<IActionResult> Edit(int ID)
        {
            var storage = await repository.GetByIDAsync(ID);
            return View(storage);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Кладовщик")]
        public IActionResult Edit(ToolRequest toolRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(toolRequest);
                }
                repository.Update(toolRequest);
                return RedirectToAction("Details", new { toolRequest.ID });
            }
            catch (Exception)
            {

            }
            return View(toolRequest);
        }

        #endregion Edit
    }
}
