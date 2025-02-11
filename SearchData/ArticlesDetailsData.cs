using Newtonsoft.Json.Linq;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.SearchData
{
    public class ArticlesDetailsData : SearchData
    {
        public Article article;
        public IEnumerable<ToolsCount> toolsCount;
        public string articleID;
        public string querry;

        public string ToString(int i)
        {
            return querry + "/{}/" + article.ID + "/{}/" + i + "/{}/" + elementsCount;
        }
        public static ArticlesDetailsData GetSearchData(string sypher)
        {
            var data = sypher.Split("/{}/");
            ArticlesDetailsData result = new ArticlesDetailsData();
            if (data.Length > 1)
                result = new ArticlesDetailsData()
                {
                    querry = data[0],
                    articleID = data[1],
                    PageNumber = Convert.ToInt32(data[1]),
                    elementsCount = Convert.ToInt32(data[2])
                };
            return result;
        }
    }
}
