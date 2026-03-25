namespace TestWebApplication.Models
{
    /// <summary>
    /// Тип данных, предоставляющий данные об отчёте
    /// </summary>
    /// <param name="Id">ID отчёта</param>
    /// <param name="Name">Название отчёта</param>
    /// <param name="Author">ID автора отчёта</param>
    /// <param name="IsTestPassed">Результат тестирования, описанного в отчёте</param>
    public class ReportHeaderModel
    {
        required public Guid Id { get; set; }

        required public string Name { get; set; }

        public Guid Author { get; set; }

        required public bool IsTestPassed { get; set; }
    }
}
