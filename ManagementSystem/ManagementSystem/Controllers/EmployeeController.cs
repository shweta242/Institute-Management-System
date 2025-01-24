using ManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly InstituteContext context;


        public EmployeeController(InstituteContext context)
        {
            this.context = context;
        }


        public async Task<IActionResult> EmployeeTable()  // Create Employee Table
        {
            var std = await context.Employees.ToArrayAsync();
            return View(std);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                await context.Employees.AddAsync(emp);
                await context.SaveChangesAsync();
                TempData["Insert"] = "Data Inserted Successfully..";
                return RedirectToAction("EmployeeTable", "Employee");
            }
            return View(emp);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || context.Employees == null)
            {
                return NotFound();
            }
            var std = await context.Employees.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            return View(std);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Employee emp)
        {
            if (id != emp.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Employees.Update(emp);
                await context.SaveChangesAsync();
                TempData["Update"] = "Data Update Successfully...";
                return RedirectToAction("EmployeeTable", "Employee");
            }
            return View(emp);
        }
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || context.Employees == null)
            {
                return NotFound();
            }
            var std = await context.Employees.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            return View(std);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, Employee emp)
        {
            var std = await context.Employees.FindAsync(id);
            if (std != null)
            {
                context.Employees.Remove(std);
            }

            await context.SaveChangesAsync();
            TempData["Delete"] = "Data Delete Successfully...";
            return RedirectToAction("EmployeeTable", "Employee");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || context.Employees == null)
            {
                return NotFound();
            }
            var stdData = await context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);
        }
    }
}
