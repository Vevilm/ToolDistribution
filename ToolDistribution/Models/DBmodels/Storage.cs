using System.ComponentModel.DataAnnotations;

namespace ToolDistribution.Models.DBmodels
{
    public class Storage
    {
        [Key]
        public short ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public string Status { get; set; }

        public Storage() { }
    }
}
