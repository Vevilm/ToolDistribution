namespace ToolDistribution.Models.DBmodels.Views
{
    public class ToolRequestsView
    {
        public int RequestID { get; set; }
        public DateTime Requested { get; set; }
        public DateTime Returned { get; set; }

        // Поля для представления элемента в поиске
        public string UserLogin { get; set; }
        public int ToolID { get; set; }
        public string ToolName { get; set; }
        public string RequestStatus { get; set; }

        // Другие поля из таблицы пользователей
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserPatronymic { get; set; }

        // Другие поля из таблицы артиклей
        public string ToolArticle { get; set; }
        public string ToolDescription { get; set; }
        public double ToolWeight { get; set; }

        // Поля из таблицы складов
        public string StorageName { get; set; }
        public string StorageType { get; set; }
    }
}