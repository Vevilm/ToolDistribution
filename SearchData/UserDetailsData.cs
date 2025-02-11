using Newtonsoft.Json.Linq;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.SearchData
{
    public class UserDetailsData : SearchData
    {
        public IEnumerable<ToolRequestsView> toolRequestsViews;
        public User user;
        public string role;
        public string querry;
        public string login;

        public string ToString(int i)
        {
            return querry + "/{}/" + login + "/{}/" + "/{}/" + i + "/{}/" + elementsCount;
        }
        public static UserDetailsData GetSearchData(string sypher)
        {
            var data = sypher.Split("/{}/");
            UserDetailsData result = new UserDetailsData();
            if (data.Length > 1)
                result = new UserDetailsData()
                {//return Login + "/{}/" + Role + "/{}/" + Status + "/{}/" + i + "/{}/" + PageSize + "/{}/" + elementsCount;
                    querry = data[0],
                    login = data[1],
                    PageNumber = Convert.ToInt32(data[2]),
                    elementsCount = Convert.ToInt32(data[3])
                };
            return result;
        }
    }
}
