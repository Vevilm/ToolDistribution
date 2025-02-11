using System.ComponentModel.DataAnnotations;

namespace ToolDistribution.Models.DBmodels
{
    public class Article
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double Weight { get; set; }

        public Article() { }
    }
}
