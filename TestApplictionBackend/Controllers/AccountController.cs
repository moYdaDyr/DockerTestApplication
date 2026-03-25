using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApplication.Data;
using TestWebApplication.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly TestWebApplicationContext _context;

        public AccountController(TestWebApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает информацию об учётной записи пользователя
        /// </summary>
        /// <param name="id">ID учётной записи пользователя</param>
        /// <returns>Информация об учётной записи пользователя</returns>
        /// <response code="200">Информация об учётной записи пользователя найдена и передана успешно</response>
        /// <response code="404">Учётная запись пользователя с таким id не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccountModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountByID(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var accountModel = await _context.AccountModel
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (accountModel == null)
                {
                    return NotFound();
                }

                return Ok(accountModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Создаёт учётную запись пользователя пользователя и возвращает её
        /// </summary>
        /// <param name="accountModel">Информация об учётной записи пользователя</param>
        /// <returns>Информация о созданной учётной записи пользователя пользователя</returns>
        /// <response code="200">Информация об учётной записи пользователя найдена и передана успешно</response>
        /// <response code="400">Предоставленные данные для учётной записи пользователя некорректны</response>
        /// <response code="404">Учётная запись пользователя с таким id не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost]
        [ProducesResponseType(typeof(AccountModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAccount([FromBody] AccountTransferModel accountModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountModel am = new AccountModel() { Id = Guid.NewGuid(), Name = accountModel.Name, Surname = accountModel.Surname, Password = accountModel.Password};

                    am.Profession = accountModel.Profession;
                    am.Password = accountModel.Password;

                    _context.Add(am);
                    await _context.SaveChangesAsync();
                    return Ok(am);
                }
                return BadRequest("Data for new account is not valid!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Обновляет учётную запись пользователя и возвращает её
        /// </summary>
        /// <param name="id">ID учётной записи пользователя</param>
        /// <param name="accountModel">Информация об учётной записи пользователя</param>
        /// <returns>Информация об учётной записи пользователя</returns>
        /// <response code="200">Информация об учётной записи пользователя найдена и передана успешно</response>
        /// <response code="400">Предоставленные данные для учётной записи пользователя некорректны</response>
        /// <response code="404">Учётная запись пользователя с таким id не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AccountModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAccount(Guid? id, [FromBody] AccountTransferModel accountModel)
        {
            try
            {
                if (ModelState.IsValid && id != null)
                {
                    AccountModel am = new AccountModel() { Id = (Guid)id, Name = accountModel.Name, Surname = accountModel.Surname, Password = accountModel.Password};

                    am.Profession = accountModel.Profession;
                    am.Password = accountModel.Password;

                    _context.Update(am);
                    await _context.SaveChangesAsync();
                    return Ok(am);
                }
                return BadRequest("Data for updated account is not valid!");
                    
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("No account with such ID!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Удаляет учётную запись пользователя и возвращает её
        /// </summary>
        /// <param name="id">ID учётной записи пользователя</param>
        /// <returns>Информация об учётной записи пользователя</returns>
        /// <response code="200">Информация об учётной записи пользователя найдена и передана успешно</response>
        /// <response code="404">Учётная запись пользователя с таким id не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(AccountModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAccount(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var accountModel = await _context.AccountModel
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (accountModel == null)
                {
                    return NotFound();
                }
                await _context.AccountModel
                    .Where(a => a.Id == id)
                    .ExecuteDeleteAsync();

                return Ok(accountModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="request">Логин и пароль пользователя</param>
        /// <returns>Сообщение об успешности авторизации</returns>
        /// <response code="200">Авторизация пользователя прошла успешно</response>
        /// <response code="400">Пароль не соответствует учётной записи</response>
        /// <response code="404">Учётная запись пользователя не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginDataModel request)
        {
            if (request == null)
            {
                return NotFound();
            }
            try
            {
                var accountModel = await _context.AccountModel
                .FirstOrDefaultAsync(m => m.Id == request.Id);
                if (accountModel == null)
                {
                    return NotFound();
                }

                if (request.Password == accountModel.Password)
                {
                    return Ok("Logged in successfully");
                }
                return BadRequest("Invalid password");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
