namespace TestWebApplication.Models
{
    /// <summary>
    /// Тип данных, предоставляющих данные при авторизации пользователя
    /// </summary>
    /// <param name="Id">ID учётной записи пользователя</param>
    /// <param name="Password">Пароль, предоставляемый пользователем</param>
    public class LoginDataModel
    {
        required public Guid Id { get; set; }

        required public string Password { get; set; }
    }
}
