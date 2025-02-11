using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ToolDistribution.Models;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.RepositoryInterfaces;
using ToolDistribution.Models.DBmodels.Views;
using static NuGet.Packaging.PackagingConstants;

namespace ToolDistribution.Repositories
{
    public class UserRepository : IUserRepository
    {
        ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async Task<User> IUserRepository.GetByIDAsync(string id)
        {
            var user = await _context.Users.AsNoTracking().FirstAsync(u => u.Id == id);
            return user;
        }

        IEnumerable<User> IUserRepository.GetSearchPage(out int totalCount, Dictionary<string, string> filters, string querry, int page)
        {
            IQueryable<User> users = _context.Users;
            if (!querry.IsNullOrEmpty())
            {
                users = _context.Users.AsNoTracking().Where(
                    a => a.Name.Contains(querry)
                    || a.UserName.Contains(querry) || a.Surname.Contains(querry));
            }
            if (!filters.IsNullOrEmpty())
            {
                users = Filter(filters, users);
            }
            totalCount = users.Count();
            if (users.Count() > SearchData.SearchData.PageSize * (page - 1))
            {
                users = users.Skip(SearchData.SearchData.PageSize * (page - 1))
                    .Take(SearchData.SearchData.PageSize);
            }
            else if (page != 1)
            {
                users = users.Take(SearchData.SearchData.PageSize);
            }
            return users.ToList();
        }

        private IQueryable<User> Filter(Dictionary<string, string> filters,
            IQueryable<User> users)
        {
            foreach (string key in filters.Keys)
            {
                switch (key.ToLower())
                {
                    case "role":
                        var roleID = _context.Roles.FirstOrDefault(r =>
                            r.NormalizedName == filters[key]).Id;
                        
                        users = users.Where(u =>
                            _context.UserRoles.Any(ur =>
                                ur.UserId == u.Id && ur.RoleId == roleID));
                        break;
                    case "status":
                        users = users.Where(s => s.Status == filters[key]);
                        break;
                    default:
                        break;
                }
            }
            return users;
        }

        bool IUserRepository.Insert(User user, string role)
        {
            _context.Users.Add(user);
            var roleID = _context.Roles.AsNoTracking().First(r => r.NormalizedName == role.ToLower()).Id;
            _context.UserRoles.Add(new IdentityUserRole<string>()
            {
                RoleId = roleID,
                UserId = user.Id
            });
            return ((IUserRepository)this).Save();
        }

        bool IUserRepository.Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }

        bool IUserRepository.Update(User user, string role)
        {
            _context.Update(user);
            var roleID = _context.Roles.AsNoTracking().First(r => r.NormalizedName == role.ToLower()).Id;
            _context.UserRoles.RemoveRange(_context.UserRoles.Where(l => l.UserId == user.Id));
            _context.UserRoles.Add(new IdentityUserRole<string>()
            {
                RoleId = roleID,
                UserId = user.Id
            });
            return ((IUserRepository)this).Save();
        }

        string IUserRepository.GetRole(User user)
        {
            var role = _context.UserRoles.First(ur => ur.UserId == user.Id).RoleId;
            return _context.Roles.First(r => r.Id == role).Name;
        }

        public IEnumerable<ToolRequestsView> GetToolsForUser(out int totalCount,
            string login, string querry = "", int page = 1)
        {
            IQueryable<ToolRequestsView> toolRequestsViews = _context.toolRequestsView;
            totalCount = toolRequestsViews.Count();
            
            return toolRequestsViews.ToList();
        }
    }
}
