using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Identity.Controllers
{
    public class CADFilesController : Controller
    {
        private readonly CadDbContext _dbContext;

        public CADFilesController(CadDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Метод для вывода списка файлов
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.CADFiles.ToListAsync());
        }

        // Метод для загрузки файла
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, int Id)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var cadFile = new CADFile
                {
                    FileName = file.FileName,
                    FilePath = filePath,
                    UploadData = DateTime.Now,
                    IdProject  = Id
                };

                _dbContext.CADFiles.Add(cadFile);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Project", "Projects", new { Id = Id });
            }

            return View("Index", await _dbContext.CADFiles.ToListAsync());
        }
        public IActionResult DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return Content("Filename not present");
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            string filePath = Path.Combine(uploadsFolder, fileName);

            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                return NotFound(); // Файл не найден
            }
        }

        // Метод для удаления файла
        public IActionResult DeleteFile(int id)
        {
            // Поиск файла в базе данных по ID
            var fileRecord = _dbContext.CADFiles.FirstOrDefault(f => f.Id == id);
            if (fileRecord == null)
            {
                return NotFound(); // Запись не найдена в базе данных
            }

            // Удаление файла с диска
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            string filePath = Path.Combine(uploadsFolder, fileRecord.FileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Удаление записи из базы данных
            _dbContext.CADFiles.Remove(fileRecord);
            _dbContext.SaveChanges();

            return RedirectToAction("Index"); // Возврат к списку файлов после удаления
        }
    }
}
