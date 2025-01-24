using ManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly InstituteContext context;
        public CourseController(InstituteContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> CourseTable()  // Create Employee Table
            {
                var std = await context.Courses.ToArrayAsync();
            
            return View(std);
            }
            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
        public async Task<IActionResult> Create(string name, string[] frontEnd, string[] backEnd, string[] dataBaseLanguage, string duration, int courseFees, string courseFormat, string description)
        {
            if (string.IsNullOrWhiteSpace(name) || courseFees <= 0 || string.IsNullOrWhiteSpace(duration) || string.IsNullOrWhiteSpace(courseFormat))
            {
                ModelState.AddModelError("", "Name, Duration, Course Fees, and Course Format are required.");
                return View();
            }

            // Save data to the database
            var courseForm = new Course
            {
                Name = name,
                FrontEnd = string.Join(",", frontEnd.Where(x => x != "false")), // Convert array to comma-separated string
                BackEnd = string.Join(",", backEnd.Where(x => x != "false")), // Convert array to comma-separated string
                DataBaseLanguage = string.Join(",", dataBaseLanguage.Where(x => x != "false")), // Convert array to comma-separated string
                Duration = duration,
                CourseFees = courseFees,
                CourseFormat = courseFormat,
                Description = description

            };

            await context.Courses.AddAsync(courseForm);
            await context.SaveChangesAsync();

            return RedirectToAction("CourseTable", "Course");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || context.Courses == null)
            {
                return NotFound();
            }
            var std = await context.Courses.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            return View(std);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Course sub)
        {
            if (id != sub.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Courses.Update(sub);
                await context.SaveChangesAsync();
                TempData["Update"] = "Data Update Successfully...";
                return RedirectToAction("CourseTable", "Course");
            }
            return View(sub);
        }
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || context.Courses == null)
            {
                return NotFound();
            }
            var std = await context.Courses.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            return View(std);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, Course emp)
        {
            var std = await context.Courses.FindAsync(id);
            if (std != null)
            {
                context.Courses.Remove(std);
            }

            await context.SaveChangesAsync();
            TempData["Delete"] = "Data Delete Successfully...";
            return RedirectToAction("CourseTable", "Course");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || context.Courses == null)
            {
                return NotFound();
            }
            var stdData = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);
        }

    }
}
