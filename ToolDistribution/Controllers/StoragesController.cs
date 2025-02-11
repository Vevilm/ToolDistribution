using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.RepositoryInterfaces;
using ToolDistribution.SearchData;

namespace ToolDistribution.Controllers
{
    [Authorize(Roles = "Администратор, Кладовщик")]
    public class StoragesController : Controller
    {
        IStorageRepository repository;

        public StoragesController(IStorageRepository repository)
        {
            this.repository = repository;
        }

        #region Index

        [HttpGet]
        public IActionResult Index(int page = 1, string searchDataStr = "")
        {
            StorageSearchData searchData = StorageSearchData.GetSearchData(searchDataStr);
            int totalCount = 0;
            Dictionary<string, string> filters = new Dictionary<string, string>();
            if (searchData.Type != "Все") filters.Add("type", searchData.Type);
            if (searchData.Status != "Все") filters.Add("status", searchData.Status);
            var storages = repository.GetSearchPage(out totalCount, filters, searchData.Name, page);
            searchData.Storages = storages;
            searchData.elementsCount = totalCount;
            return View(searchData);
        }

        [HttpPost]
        public IActionResult Index(StorageSearchData searchData)
        {
            int totalCount = 0;
            Dictionary<string, string> filters = new Dictionary<string, string>();
            if (searchData.Type != "Все") filters.Add("type", searchData.Type);
            if (searchData.Status != "Все") filters.Add("status", searchData.Status);
            var storages = repository.GetSearchPage(out totalCount, filters, searchData.Name);
            searchData.Storages = storages;
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
        public IActionResult Create(Storage storage)
        {
            if (!ModelState.IsValid)
            {
                return View(storage);
            }
            repository.Insert(storage);
            return RedirectToAction("Index");
        }

        #endregion Create

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Storage storage = await repository.GetByIDAsync(id);
            return View(storage);
        }

        #endregion Details

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int ID)
        {
            var storage = await repository.GetByIDAsync(ID);
            return View(storage);
        }

        [HttpPost]
        public IActionResult Edit(Storage storage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(storage);
                }
                repository.Update(storage);
                return RedirectToAction("Details", new { storage.ID });
            }
            catch (Exception)
            {

            }
            return View(storage);
        }

        #endregion Edit
    }
}
