using ManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Controllers
{
    public class AdmissionController : Controller
    {
        private readonly InstituteContext context;
        public AdmissionController(InstituteContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> AdmissionTable(string searchString, DateOnly? fromDate, DateOnly? toDate)  // Create Admission Table
        {

            // Start by selecting all entities
            var entities = from e in context.Students
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
            // If both fromDate and toDate are of type DateTime, no need to convert DateOnly to DateTime
            if (fromDate.HasValue && toDate.HasValue)
            {
                // Convert DateOnly to DateTime (with midnight time)
                DateTime fromDateTime = fromDate.Value.ToDateTime(TimeOnly.MinValue);
                DateTime toDateTime = toDate.Value.ToDateTime(TimeOnly.MinValue);

                entities = entities.Where(e => e.AdmissionDate >= fromDateTime && e.AdmissionDate <= toDateTime);
            }
            entities = entities.Include(a => a.CourseNavigation);

           entities = entities.OrderByDescending(a => a.Id);

            // Return the filtered data to the view
            
            var totalCollection = entities.Sum(a => a.ReceiveAmount);
            var totalPending = entities.Sum(a => a.PendingAmount);
            ViewData["totalCollection"] = totalCollection;
            ViewData["totalPending"] = totalPending;
            return View(await entities.ToListAsync());
        }

    
    [HttpGet("Admission/add/{id}")]
        public async Task<IActionResult> Add(int id)   //convert inquiry to addmission 
        {
            var inquiry = await context.Inquiries.FindAsync(id);
            if (inquiry == null)
            {
                return NotFound();
            }

            var admission = new Student
            {
                Name = inquiry.Name,
                Gender=inquiry.Gender,
                Dob=inquiry.Dob,
                City = inquiry.City,
                ContactNo = inquiry.Contact,
                Qualification = inquiry.Qualification,
                CurrentAddress = inquiry.Address,
                Email = inquiry.Email,
                InqiuryId = inquiry.Id
            };

            return View(admission);


        }
        [HttpPost]
        public async Task<IActionResult> Add(Student admission)
        {
            if (ModelState.IsValid)
            {

                admission.Id = 0;
                await context.Students.AddAsync(admission);
                await context.SaveChangesAsync();

                TempData["Insert"] = "Data Inserted Successfully.";
                return RedirectToAction("AdmissionTable", "Admission");
            }

            return View(admission);
        }







        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(context.Courses, "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student std)
        {
            if (ModelState.IsValid)
            {
                await context.Students.AddAsync(std);
                await context.SaveChangesAsync();
                TempData["Insert"] = "Data Inserted Successfully..";
                return RedirectToAction("AdmissionTable", "Admission");
            }
            ViewBag.Courses = new SelectList(context.Courses, "Name", "Name");
            return View(std);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || context.Students == null)
            {
                return NotFound();
            }
            var std = await context.Students.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            ViewBag.Courses = new SelectList(context.Courses, "Name", "Name");
            return View(std);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Student std)
        {
            if (id != std.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Students.Update(std);
                await context.SaveChangesAsync();
                TempData["Update"] = "Data Update Successfully...";
                return RedirectToAction("AdmissionTable", "Admission");
            }
            ViewBag.Courses = new SelectList(context.Courses, "Name", "Name");
            return View(std);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || context.Students == null)
            {
                return NotFound();
            }

            var std = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (std == null)
            {
                return NotFound();
            }

            return View(std); // Correct variable used here
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var std = await context.Students.FindAsync(id); // Fetch object to delete from DB
            if (std != null) // Check the correct object for null
            {
                context.Students.Remove(std); // Remove the fetched object
                await context.SaveChangesAsync();
                TempData["Delete"] = "Data Deleted Successfully...";
            }
            else
            {
                TempData["Delete"] = "Failed to delete the data. It may not exist.";
            }

            return RedirectToAction("AdmissionTable", "Admission");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || context.Students == null)
            {
                return NotFound();
            }
            var stdData = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);
        }
        
    }

}

