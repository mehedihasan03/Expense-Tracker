using Expense_Tracker.Attributes.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    
    public class ExpenseCategory
    {
        [Key]
        
        public int ExpenseCategoryId { get; set; }

        [Required(ErrorMessage = "Please Add Name")]      
        public string ExpenseCategoryName { get; set; }

        public virtual IList<Expense> Expenses { get; set; }
    }
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }
        [Display(Name ="Catagory")]
        public int ExpenseCategoryId_FK { get; set; } 
        
        [Required, Today, DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Date Of Expenses")]
        public DateTime ExpenseDate { get; set; }

        [Required(ErrorMessage ="Fill The Field")]
        [Display(Name ="Expense Per Taka")]
        public decimal ExpenseAmount { get; set; }

        [ForeignKey("ExpenseCategoryId_FK")]
        public ExpenseCategory ExpenseCategories { get; set; }
    }
}
