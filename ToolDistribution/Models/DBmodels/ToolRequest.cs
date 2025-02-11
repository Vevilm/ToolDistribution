using System.ComponentModel.DataAnnotations;

namespace ToolDistribution.Models.DBmodels
{
    public class ToolRequest
    {
        [Key]
        public int ID { get; set; }
        public string WorkerID { get; set; }
        public int ToolID { get; set; }
        public DateTime Requested { get; set; }
        public DateTime Returned { get; set; }

        public string Status { get; set; }

        public ToolRequest() { }
    }
}
