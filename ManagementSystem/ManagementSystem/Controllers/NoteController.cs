using Humanizer;
using ManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace InstituteManagement.Controllers
{
    public class NoteController : Controller
    {
        private readonly InstituteContext _context;

        public NoteController(InstituteContext context)
        {
            _context = context;
        }


        // Display uploaded notes
        public async Task<IActionResult> NotesTable()
        {
            var notes = await _context.Notes.ToListAsync();
            return View(notes);
        }

        // GET: Notes/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Notes/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var fileType = Path.GetExtension(file.FileName);
                var fileSize = file.Length;
                var uploadDate = DateTime.Now;

                // Create the uploads folder if it doesn't exist
                var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                // Define the file path
                var filePath = Path.Combine(uploadFolderPath, fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Create a new note entry for the database
                var note = new Note
                {
                    FileName = fileName,
                    FileType = fileType,
                    UploadDate = uploadDate
                };

                // Save the note details to the database
                _context.Add(note);
                await _context.SaveChangesAsync();

                TempData["Success"] = "File uploaded successfully!";
                return RedirectToAction(nameof(NotesTable)); // Redirect to Notes table after success
            }

            TempData["Error"] = "Please select a file to upload!";
            return View();
        }

        public IActionResult Download(int id)
        {
            // Fetch the note from the database
            var note = _context.Notes.FirstOrDefault(n => n.FileId == id);
            if (note == null)
            {
                TempData["Error"] = "File not found!";
                return RedirectToAction(nameof(NotesTable));
            }

            // Define the file path
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", note.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                TempData["Error"] = "File not found on server!";
                return RedirectToAction(nameof(NotesTable));
            }

            // Return the file for download
            return File(System.IO.File.ReadAllBytes(filePath), "application/octet-stream", note.FileName);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.FileId == id);
            if (note == null)
            {
                TempData["Error"] = "File not found!";
                return RedirectToAction(nameof(NotesTable));
            }

            // Define the file path
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", note.FileName);

            // Delete the file from the server
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Delete the record from the database
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            TempData["Success"] = "File deleted successfully!";
            return RedirectToAction(nameof(NotesTable));
        }



    }
}
