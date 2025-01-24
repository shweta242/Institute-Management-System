using ManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Controllers
{
   
    public class InquiryController : Controller
    {
        private readonly InstituteContext context;
        public InquiryController(InstituteContext context)
        {
               this.context = context;
        }
        public async Task<IActionResult> Table(string searchString, DateOnly? fromDate, DateOnly? toDate)
        {
            // Start by selecting all entities
            var entities = from e in context.Inquiries
                           select e;

            // Search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                entities = entities.Where(e => e.Name.Contains(searchString) ||
                                                
                                                 e.City.Contains(searchString) ||
                                                 e.Dob.Contains(searchString) ||
                                                 e.Qualification.Contains(searchString) ||
                                                 e.Email.Contains(searchString) ||
                                                 e.Course.Contains(searchString) ||
                                                 e.Gender.Contains(searchString));
            }

            // Date range filter
            if (fromDate.HasValue && toDate.HasValue)
            {
                entities = entities.Where(e => e.InquiryDate >= fromDate.Value && e.InquiryDate <= toDate.Value);
            }
            entities = entities.Include(a => a.CourseNavigation);

            entities = entities.OrderByDescending(s => s.Id);

            // Return the filtered data to the view
            return View(await entities.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Courses=new SelectList(context.Courses,"Name","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                await context.Inquiries.AddAsync(inquiry);
                await context.SaveChangesAsync();
                TempData["Insert"] = "Data Inserted Successfully..";
                return RedirectToAction("Table", "Inquiry");
            }
            ViewBag.Courses = new SelectList(context.Courses, "Name", "Name");
            return View(inquiry);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || context.Inquiries == null)
            {
                return NotFound();
            }
            var std = await context.Inquiries.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            ViewBag.Courses = new SelectList(context.Courses, "Name", "Name");
            return View(std);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Inquiry inquiry)
        {
            if (id != inquiry.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Inquiries.Update(inquiry);
                await context.SaveChangesAsync();
                TempData["Update"] = "Data Update Successfully...";
                return RedirectToAction("Table", "Inquiry");
            }
            ViewBag.Courses = new SelectList(context.Courses, "Name", "Name");

            return View(inquiry);
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || context.Inquiries == null)
            {
                return NotFound();
            }
            var std = await context.Inquiries.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            return View(std);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, Inquiry inquiry)
        {
            var std = await context.Inquiries.FindAsync(id);
            if (std != null)
            {
                context.Inquiries.Remove(std);
            }

            await context.SaveChangesAsync();
            TempData["Delete"] = "Data Delete Successfully...";
            return RedirectToAction("Table", "Inquiry");
        }
    }
}
