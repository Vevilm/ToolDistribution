using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.SearchData 
{
    public class ArticleSearchData : SearchData
    {
        public IEnumerable<Article> Articles { get; set; }
        public string Value { get; set; } = "";
        public IEnumerable<ArticlesToToolsCount> statistics { get; set; }

        public string ToString(int i)
        {
            return Value + "/{}/" + i + "/{}/" + elementsCount;
        }
        public static ArticleSearchData GetSearchData(string sypher)
        {
            var data = sypher.Split("/{}/");
            ArticleSearchData result = new ArticleSearchData();
            if (data.Length > 1)
                result = new ArticleSearchData()
                {
                    Value = data[0],
                    PageNumber = Convert.ToInt32(data[1]),
                    elementsCount = Convert.ToInt32(data[2])
                };
            return result;
        }
    }
}
