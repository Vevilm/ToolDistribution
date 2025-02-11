using System.ComponentModel.DataAnnotations;

namespace ToolDistribution.Models.DBmodels
{
    public class Tool
    {
        [Key]
        public int ID { get; set; }
        public short StorageID { get; set; }
        public string ArticleID { get; set; }

        public string Status { get; set; }

        public Tool() { }
    }
}
