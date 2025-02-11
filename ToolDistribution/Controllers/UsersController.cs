using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.RepositoryInterfaces;
using ToolDistribution.SearchData;

namespace ToolDistribution.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        IUserRepository repository;

        public UsersController(IUserRepository repository)
        {
            this.repository = repository;
        }

        #region Index

        [HttpGet]
        [Authorize(Roles ="Администратор")]
        public IActionResult Index(int page = 1, string searchDataStr = "")
        {
            UserSearchData searchData = UserSearchData.GetSearchData(searchDataStr);
            int totalCount = 0;
            Dictionary<string, string> filters = new Dictionary<string, string>();
            if (searchData.Role != "Все") filters.Add("role", searchData.Role);
            if (searchData.Status != "Все") filters.Add("status", searchData.Status);
            var storages = repository.GetSearchPage(out totalCount, filters, searchData.Login, page);
            searchData.Users = storages;
            searchData.elementsCount = totalCount;
            return View(searchData);
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost]
        public IActionResult Index(UserSearchData searchData)
        {
            int totalCount = 0;
            Dictionary<string, string> filters = new Dictionary<string, string>();
            if (searchData.Role != "Все") filters.Add("role", searchData.Role);
            if (searchData.Status != "Все") filters.Add("status", searchData.Status);
            var users = repository.GetSearchPage(out totalCount, filters, searchData.Login);
            searchData.Users = users;
            searchData.elementsCount = totalCount;
            return View(searchData);
        }

        #endregion Index

        //#region Create

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return RedirectToPage($"//Account/Register");
        //}

        //[HttpPost]
        //public IActionResult Create(UserEditData userEditData)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(userEditData);
        //    }
        //    repository.Insert(userEditData.user, userEditData.role);
        //    return RedirectToAction("Details", new { userEditData.user.Id });
        //}

        //#endregion Create

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            User userLocal = await repository.GetByIDAsync(id);
            string roleLocal = repository.GetRole(userLocal);
            int count;
            UserDetailsData userDetailsData = new UserDetailsData()
            {
                user = userLocal,
                role = roleLocal,
                toolRequestsViews = repository.GetToolsForUser(out count, userLocal.UserName)
            };
            return View(userDetailsData);
        }
        //[HttpPost]
        //public async Task<IActionResult> Details(string id, string detailsData)
        //{
        //    User userLocal = await repository.GetByIDAsync(id);
        //    string roleLocal = repository.GetRole(userLocal);
        //    int count;
        //    UserDetailsData userDetailsData = UserDetailsData.GetSearchData(detailsData);
        //    userDetailsData.toolRequestsViews = repository.GetToolsForUser(out count, userLocal.UserName,
        //        userDetailsData.querry, userDetailsData.PageNumber);
        //    return View(userDetailsData);
        //}

        #endregion Details

        #region Edit

        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public async Task<IActionResult> Edit(string ID)
        {
            var user = await repository.GetByIDAsync(ID);
            return View(user);
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost]
        public IActionResult Edit(UserEditData userEditData)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(userEditData);
                }
                repository.Update(userEditData.user, userEditData.role);
                return RedirectToAction("Details", new { userEditData.user.Id });
            }
            catch (Exception)
            {

            }
            return View(userEditData);
        }

        #endregion Edit
    }
}
