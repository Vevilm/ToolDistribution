namespace ToolDistribution.Models.DBmodels.Views
{
    public class ToolsCount
    {
        public short StorageID { get; set; }
        public string ArticleID { get; set; }
        public string Storage { get; set; }
        public string Article { get; set; }
        public int avaliable { get; set; }
        public int total { get; set; }


        public ToolsCount() { }
    }
}
