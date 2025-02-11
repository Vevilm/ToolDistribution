using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Contracts;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.SearchData
{
    public class RequestData
    {
        public List<SelectListItem> storages { get; set; }
        public Article article { get; set; }
        public string userID { get; set; }
        public string storageID { get; set; }
        public string articleID { get; set; }

        public override string ToString()
        {
            return articleID + "/{}/" + storageID + "/{}/" + userID;
        }

        public static RequestData Decode(string code) {
            var data = code.Split("/{}/");
            RequestData result = new RequestData();
            if (data.Length > 1)
                result = new RequestData()
                {
                    articleID = data[0],
                    storageID = data[1],
                    userID = data[2]
                };
            return result;
        }
    }
}
