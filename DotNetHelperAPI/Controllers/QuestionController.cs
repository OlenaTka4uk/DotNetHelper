using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetHelperAPI.Data;
using DotNetHelperAPI.Models;

namespace DotNetHelperAPI.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Question
        public async Task<IActionResult> Index()
        {
              return _context.QuestionModel != null ? 
                          View(await _context.QuestionModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.QuestionModel'  is null.");
        }

        public async Task<IActionResult> ShowSearchForm()
        {
            return _context.QuestionModel != null ?
                        View() :
                        Problem("Entity set 'ApplicationDbContext.QuestionModel'  is null.");
        }

        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index", await _context.QuestionModel.Where(x => x.Question.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: Question/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.QuestionModel == null)
            {
                return NotFound();
            }

            var questionModel = await _context.QuestionModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionModel == null)
            {
                return NotFound();
            }

            return View(questionModel);
        }

        // GET: Question/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Question/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] QuestionModel questionModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionModel);
        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.QuestionModel == null)
            {
                return NotFound();
            }

            var questionModel = await _context.QuestionModel.FindAsync(id);
            if (questionModel == null)
            {
                return NotFound();
            }
            return View(questionModel);
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] QuestionModel questionModel)
        {
            if (id != questionModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionModelExists(questionModel.Id))
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
            return View(questionModel);
        }

        // GET: Question/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.QuestionModel == null)
            {
                return NotFound();
            }

            var questionModel = await _context.QuestionModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionModel == null)
            {
                return NotFound();
            }

            return View(questionModel);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.QuestionModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.QuestionModel'  is null.");
            }
            var questionModel = await _context.QuestionModel.FindAsync(id);
            if (questionModel != null)
            {
                _context.QuestionModel.Remove(questionModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionModelExists(int id)
        {
          return (_context.QuestionModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
