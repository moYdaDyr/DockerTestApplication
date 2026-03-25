namespace TestWebApplication.Models
{
    /// <summary>
    /// Тип данных, предоставляющий данные об учётной записи пользователя
    /// </summary>
    /// <param name="Name">Имя пользователя</param>
    /// <param name="Surname">Фамилия пользователя</param>
    /// <param name="Profession">Должность пользователя</param>
    /// <param name="Password">Хэш-код пароля пользователя</param>
        public class AccountTransferModel
        {
            required public string Name { get; set; }

            required public string Surname { get; set; }

            public string Profession { get; set; }

            required public string Password { get; set; }
        }
}
