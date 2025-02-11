using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.SearchData
{
    public class ToolRequestSearchData : SearchData
    {
        public IEnumerable<ToolRequestsView> ToolRequests { get; set; }
        public string Field { get; set; } = "";
        public string Value { get; set; } = "";
        public string Status { get; set; } = "Все";

        public string ToString(int i)
        {
            return Field + "/{}/" + Value + "/{}/" + Status + "/{}/" + i + "/{}/" + elementsCount;
        }
        public static ToolRequestSearchData GetSearchData(string sypher)
        {
            var data = sypher.Split("/{}/");
            ToolRequestSearchData result = new ToolRequestSearchData();
            if (data.Length > 1)
                result = new ToolRequestSearchData()
                {
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
