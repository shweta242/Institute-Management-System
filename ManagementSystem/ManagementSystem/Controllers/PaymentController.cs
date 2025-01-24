using ManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Controllers
{
    public class PaymentController : Controller
    {
        public readonly InstituteContext context;
        public PaymentController(InstituteContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Click()
        {
            // Use async for fetching data from the database
            var admissions = await context.Students
                .Select(a => new
                {
                    Text = $"{a.Name} - {a.Id} - {a.ContactNo} - {a.ParentName}",
                    Value = a.Id
                })
                .ToListAsync(); // Use async method to fetch the data

            // Assign the result to ViewBag
            ViewBag.Admissions = admissions;

            return View();
        }


        public async Task<IActionResult> Payment(string searchString, DateOnly? fromDate, DateOnly? toDate)
        {
            var payments =context.Payments.AsQueryable();

            //apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                payments = payments.Where(p => p.StudentName.Contains(searchString) ||
                                           p.Gmail.Contains(searchString) ||
                                           p.MobileNo.Contains(searchString));
            }
            //apply date range filter
            if(fromDate.HasValue && toDate.HasValue)
            {
                payments = payments.Where(p => p.TransactionDate >= fromDate && p.TransactionDate <= toDate);
            }
            //order by pid in descending oeder
            payments = payments.OrderByDescending(p => p.Pid);
            return View(await payments.ToListAsync());
        }

        //edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || context.Payments == null)
            {
                return NotFound();
            }
            var std = await context.Payments.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
           
            return View(std);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Payment pay)
        {
            if (id != pay.Pid)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Payments.Update(pay);
                await context.SaveChangesAsync();
                TempData["Edit"] = "Data Edit Successfully...";
                return RedirectToAction("Payment", "Payment");
            }
            

            return View(pay);
        }

        //delete

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || context.Payments == null)
            {
                return NotFound();
            }
            var std = await context.Payments.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            return View(std);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, Payment pay)
        {
            var std = await context.Payments.FindAsync(id);
            if (std != null)
            {
                context.Payments.Remove(std);
            }

            await context.SaveChangesAsync();
            TempData["Delete"] = "Data Delete Successfully...";
            return RedirectToAction("Payment", "Payment");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || context.Payments == null)
            {
                return NotFound();
            }
            var stdData = await context.Payments.FirstOrDefaultAsync(x => x.Pid == id);
            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);
        }


        public async Task<IActionResult> Make(int id) // Use async for the method
        {
            // Fetch student data asynchronously
            var student = await context.Students
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    Sid = a.Id,
                    StudentName = a.Name,
                    MobileNo = a.ContactNo,
                    Parent = a.ParentName,
                    Gmail = a.Email,
                    CourseName = a.Course,
                    Fees = a.DiscountedCourseFees
                })
                .FirstOrDefaultAsync(); // Use async method

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            var model = new Payment
            {
                StudentId = student.Sid,
                StudentName = student.StudentName,
                MobileNo = student.MobileNo,
                ParentName = student.Parent,
                Gmail = student.Gmail,
                CourseName = student.CourseName,
                FeesStatus = student.Fees,
                ReceiptNo = GenerateReceiptNumber()
            };

            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Make(Payment pay)
        {
            if (ModelState.IsValid)
            {
                var admission = await context.Students.FirstOrDefaultAsync(a => a.Id == pay.StudentId);
                if (admission == null)
                {
                    return NotFound("Admission Record Not Found");
                }
                var totalReceivedAmount = context.Payments
                                            .Where(p => p.StudentId == pay.StudentId)
                                            .Sum(p => (decimal?)p.RecievedAmount) ?? 0;
                var remainingFees = pay.FeesStatus - pay.RecievedAmount;
                if (remainingFees < 0)
                {
                    ModelState.AddModelError("", "Recieved Amount Exceeds the outstanding fees..");
                    return View(pay);
                }

                var payment = new Payment
                {
                    StudentId = pay.StudentId,
                    StudentName = pay.StudentName,
                    MobileNo = pay.MobileNo,
                    ParentName = pay.ParentName,
                    Gmail = pay.Gmail,
                    CourseName = pay.CourseName,
                    FeesStatus = pay.FeesStatus,
                    ReceiptDate = pay.ReceiptDate == default ? DateOnly.FromDateTime(DateTime.Now) : pay.ReceiptDate,
                    Narration = pay.Narration ?? "Default Narration",
                    ReceiptNo = pay.ReceiptNo ?? GenerateReceiptNumber(),
                    ReceiptRemark = pay.ReceiptRemark ?? "No Remark",
                    ReceivedBy = pay.ReceivedBy ?? "Admin",
                    RecievedMode = pay.RecievedMode ?? "Cash",
                    TermCondition = pay.TermCondition ?? "Agree",
                    TransactionNumber = pay.TransactionNumber,
                    TransactionStatus = pay.TransactionStatus ?? "Pending",
                    RecievedAmount = pay.RecievedAmount > 0 ? pay.RecievedAmount : 0,  // Ensure ReceivedAmount is not negative
                    PendingAmount=pay.PendingAmount > 0 ? pay.PendingAmount : 0,
                    TransactionDate = pay.TransactionDate ?? DateOnly.FromDateTime(DateTime.Now)  // Default to current date if TransactionDate is null
                };


                context.Payments.Add(payment); // Add the new payment to the context
                admission.ReceiveAmount = totalReceivedAmount + pay.RecievedAmount;
                admission.PendingAmount = remainingFees;


                await context.SaveChangesAsync(); // Use async SaveChanges

                return RedirectToAction("Payment", "Payment"); // Redirect to the Payment action
            }

            return View(pay); // Return the view with the payment data in case of invalid model state
        }

        private string GenerateReceiptNumber()
        {
            // Get the current year and next year to represent the academic year (e.g., 2023-2024)
            var currentYear = DateTime.Now.Year;
            var nextYear = currentYear + 1;
            var academicYear = $"{currentYear.ToString().Substring(2, 2)}-{nextYear.ToString().Substring(2, 2)}"; // Example: "23-24"

            // Generate the sequential number (this could be based on the latest receipt number in the database)
            var receiptCount = context.Payments.Count(); // Assuming you're getting this from your database
            var sequentialNumber = receiptCount + 1; // Increment based on the existing records

            // Format the receipt number like "TEPL/REC/23-24/3788"
            return $"TEPL/REC/{academicYear}/{sequentialNumber}";
        }


    }
}
   