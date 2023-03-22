using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Controllers
{
    public class ExpenseCategoriesController : Controller
    {
        public ApplicationDbContext applicationDb;
        public ExpenseCategoriesController(ApplicationDbContext applicationDbContext)
        {
            applicationDb = applicationDbContext;
        }
        public IActionResult Index()
        {
            return View(applicationDb.ExpenseCategories.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseCategory expenseCategory)
        {
            if (ModelState.IsValid)
            {
                
                applicationDb.ExpenseCategories.Add(expenseCategory);

                await applicationDb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenseCategory);

        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var expenseCategory = await applicationDb.ExpenseCategories.FindAsync(id);
            if (expenseCategory == null)
            {
                return NotFound();
            }
            return View(expenseCategory);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, ExpenseCategory expenseCategory)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationDb.Update(expenseCategory);
                    await applicationDb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseCategoryExists(expenseCategory.ExpenseCategoryId))
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
            return View(expenseCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseCategory = await applicationDb.ExpenseCategories
                .FirstOrDefaultAsync(m => m.ExpenseCategoryId == id);
            if (expenseCategory == null)
            {
                return NotFound();
            }

            return View(expenseCategory);
        }
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expenseCategory = await applicationDb.ExpenseCategories.FindAsync(id);
            applicationDb.ExpenseCategories.Remove(expenseCategory);
            await applicationDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ExpenseCategoryExists(int id)
        {
            return applicationDb.ExpenseCategories.Any(p => p.ExpenseCategoryId == id);
        }
    }
}
