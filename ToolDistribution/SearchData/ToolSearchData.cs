using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.SearchData
{
    public class ToolSearchData : SearchData
    {
        public IEnumerable<ToolsView> Tools { get; set; }
        public string Field { get; set; } = "";
        public string Value { get; set; } = "";
        public string Status { get; set; } = "Все";

        public string ToString(int i)
        {
            return Field + "/{}/" + Value + "/{}/" + Status + "/{}/" + i + "/{}/" + elementsCount;
        }
        public static ToolSearchData GetSearchData(string sypher)
        {
            var data = sypher.Split("/{}/");
            ToolSearchData result = new ToolSearchData();
            if (data.Length > 1)
                result = new ToolSearchData()
                {//return Login + "/{}/" + Role + "/{}/" + Status + "/{}/" + i + "/{}/" + PageSize + "/{}/" + elementsCount;
                    Field = data[0],
                    Value = data[1],
                    Status = data[2],
                    PageNumber = Convert.ToInt32(data[3]),
                    elementsCount = Convert.ToInt32(data[4])
                };
            return result;
        }
    }
}
