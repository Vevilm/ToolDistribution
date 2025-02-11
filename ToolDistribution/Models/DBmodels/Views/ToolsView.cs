namespace ToolDistribution.Models.DBmodels.Views
{
    public class ToolsView
    {
        public int ID {  get; set; }
     
        // Поля для представления элемента в поиске
        public string Name { get; set; }
        public string Storage { get; set; }
        public string Status { get; set; }

        // Остальные поля из таблицы артиклей
        public string Article { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }

        // Остальные поля из таблицы складов
        public short StorageID { get; set; }
        public string Type { get; set; }
        public string StorageStatus { get; set; }
    }
}
