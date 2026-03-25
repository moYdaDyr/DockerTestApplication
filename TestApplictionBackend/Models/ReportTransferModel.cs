namespace TestWebApplication.Models
{
    /// <summary>
    /// Тип данных, предоставляющий данные об отчёте
    /// </summary>
    /// <param name="Name">Название отчёта</param>
    /// <param name="Text">текст отчёта</param>
    /// <param name="Author">ID автора отчёта</param>
    /// <param name="IsTestPassed">Результат тестирования, описанного в отчёте</param>
    public class ReportTransferModel
    {
        required public string Name { get; set; }

        public string Text { get; set; }

        public Guid Author { get; set; }

        required public bool IsTestPassed { get; set; }
    }
}
