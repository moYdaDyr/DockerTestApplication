using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestWebApplication.Data;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Produces("application/json")]
    public class ReportController : Controller
    {
        private readonly TestWebApplicationContext _context;

        public ReportController(TestWebApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение краткой информации о всех отчётах, составленных одним автором
        /// </summary>
        /// <param name="author">ID автора отчёта</param>
        /// <returns>Список с базовой информацией (ID, название и автор) всех отчётов</returns>
        /// <response code="200">Информация об отчётах успешно собраны и отправлены</response>
        /// <response code="404">Автор не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet()]
        [ProducesResponseType(typeof(List<ReportHeaderModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReportsByAuthor([FromBody] Guid author)
        {
            try
            {
                var isAuthorThere = await _context.AccountModel.AnyAsync(c => c.Id == author);

                if (!isAuthorThere)
                {
                    return NotFound();
                }

                return Ok(await _context.ReportModel.Select(c => new ReportHeaderModel() { Id=c.Id, Name=c.Name, Author=c.Author, IsTestPassed=c.IsTestPassed }).Where(d => d.Author == author).ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(List<ReportHeaderModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                return Ok(await _context.ReportModel.Select(c => new ReportHeaderModel() { Id = c.Id, Name = c.Name, Author = c.Author, IsTestPassed = c.IsTestPassed }).ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Возвращает информацию об отчёте
        /// </summary>
        /// <param name="id">ID отчёта</param>
        /// <returns>Информация об отчёте</returns>
        /// <response code="200">Информация об отчёте найдена и передана успешно</response>
        /// <response code="404">Отчёт с таким ID не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReportModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportByID(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var reportModel = await _context.ReportModel
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (reportModel == null)
                {
                    return NotFound();
                }

                return Ok(reportModel);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Создаёт отчёт и возвращает его
        /// </summary>
        /// <param name="reportModel">Информация об отчёте</param>
        /// <returns>Информация о созданном отчёте</returns>
        /// <response code="200">Информация об отчёте создана и передана успешно</response>
        /// <response code="400">Предоставленные данные для отчёта некорректны</response>
        /// <response code="404">Отчёт с таким id не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost]
        [ProducesResponseType(typeof(ReportModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReport([FromBody] ReportTransferModel reportModel)
        {
            try
            {
                ReportModel rtm = new ReportModel(){ Id = Guid.NewGuid(), Name = reportModel.Name, IsTestPassed = reportModel.IsTestPassed};

                if (ModelState.IsValid)
                {
                    rtm.Text = reportModel.Text;
                    rtm.Author = reportModel.Author;

                    _context.Add(rtm);
                    await _context.SaveChangesAsync();
                    return Ok(rtm);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Обновляет информацию об отчёте и возвращает его
        /// </summary>
        /// <param name="id">ID отчёта</param>
        /// <param name="reportModel">Информация об отчёте</param>
        /// <returns>Информация об обновлённом отчёте</returns>
        /// <response code="200">Информация об отчёте обновлена и передана успешно</response>
        /// <response code="400">Предоставленные данные для отчёта некорректны</response>
        /// <response code="404">Отчёт с таким id не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReportModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateReport(Guid? id, [FromBody] ReportTransferModel reportModel)
        {
            try
            {
                if (ModelState.IsValid && id!=null)
                {
                    ReportModel rtm = new ReportModel(){ Id = (Guid)id, Name = reportModel.Name, IsTestPassed = reportModel.IsTestPassed};

                    rtm.Text = reportModel.Text;
                    rtm.Author = reportModel.Author;

                    _context.Update(rtm);
                    await _context.SaveChangesAsync();
                    return Ok(rtm);
                }
                return BadRequest();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        /// <summary>
        /// Удаляет информацию об отчёте и возвращает его
        /// </summary>
        /// <param name="id">ID отчёта</param>
        /// <returns>Информация об удалённом отчёте</returns>
        /// <response code="200">Информация об отчёте удалена и передана успешно</response>
        /// <response code="404">Отчёт с таким id не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ReportModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteReport(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var reportModel = await _context.ReportModel
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (reportModel == null)
                {
                    return NotFound();
                }
                await _context.ReportModel
                    .Where(a => a.Id == id)
                    .ExecuteDeleteAsync();

                return Ok(reportModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
