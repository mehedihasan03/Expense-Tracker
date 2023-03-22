using Expense_Tracker.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Expense_Tracker.Controllers
{
    public class ExpensesController : Controller
    {
        public ApplicationDbContext _applicationDb;
        IWebHostEnvironment _webHostEnvironment;

        public ExpensesController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _applicationDb = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index(DateTime? startDate , DateTime? endDate)
        {
            var AppDb = _applicationDb.Expenses.Include(n => n.ExpenseCategories);
            if (startDate.HasValue && endDate.HasValue)
            {
                return View(
                    AppDb.Where(w => w.ExpenseDate >= startDate && w.ExpenseDate <= endDate).AsEnumerable()
                    ); 
            }
            else
            {
                return View(await AppDb.ToListAsync()) ;
            }   
        }

        public IActionResult Create()
        {
            ViewData["ExpenseCategoryId_FK"] = new SelectList(_applicationDb.ExpenseCategories, "ExpenseCategoryId", "ExpenseCategoryName");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {                
                _applicationDb.Add(expense);
                await _applicationDb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseCategoryId_FK"] = new SelectList(_applicationDb.ExpenseCategories, "ExpenseCategoryId", "ExpenseCategoryName", expense.ExpenseCategoryId_FK);
            return View(expense);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var expense = await _applicationDb.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["ExpenseCategoryId_FK"] = new SelectList(_applicationDb.ExpenseCategories, "ExpenseCategoryId", "ExpenseCategoryName", expense.ExpenseCategoryId_FK);
            return View(expense);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {                
                try
                {
                    _applicationDb.Update(expense);
                    await _applicationDb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseCategoryId_FK"] = new SelectList(_applicationDb.ExpenseCategories, "ExpenseCategoryId", "ExpenseCategoryName", expense.ExpenseCategoryId_FK);
            return View(expense);
        }

        public ActionResult Delete(int? id)
        {
            try
            {

                var firstEntity = _applicationDb.Expenses.Where(c => c.ExpenseId == id).FirstOrDefault();

                _applicationDb.Expenses.Remove(firstEntity);

                _applicationDb.SaveChanges();

            }

            finally
            {
            }

            return RedirectToAction("Index");
        }

        private bool ExpenseExists(int id)
        {
            return _applicationDb.Expenses.Any(b => b.ExpenseId == id);
        }
    }
}
