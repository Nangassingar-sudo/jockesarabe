using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesWebApp.Data;
using JokesWebApp.Models;
using System.Security.Claims;

namespace JokesWebApp.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jokes
        public async Task<IActionResult> Index(string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;

            var jokes = _context.Jokes
                .Include(j => j.JokeQuestion)
                .Include(j => j.JokeAnswer)
                .Include(j => j.User)
                .Include(j => j.Comments)
                    .ThenInclude(c => c.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                jokes = jokes.Where(j => 
                    j.JokeQuestion!.Text.Contains(searchTerm) || 
                    j.JokeAnswer!.Text.Contains(searchTerm));
            }

            return View(await jokes.ToListAsync());
        }

        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Jokes
                .Include(j => j.JokeQuestion)
                .Include(j => j.JokeAnswer)
                .Include(j => j.User)
                .Include(j => j.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // GET: Jokes/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.JokeQuestionId = new SelectList(await _context.JokeQuestions.ToListAsync(), "Id", "Text");
            ViewBag.JokeAnswerId = new SelectList(await _context.JokeAnswers.ToListAsync(), "Id", "Text");
            return View();
        }

        // POST: Jokes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("JokeQuestionId,JokeAnswerId")] Joke joke)
        {
            if (ModelState.IsValid)
            {
                // Automatically associate the current user
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    joke.UserId = userId;
                }
                else
                {
                    ModelState.AddModelError("", "User not found. Please log in again.");
                    ViewBag.JokeQuestionId = new SelectList(await _context.JokeQuestions.ToListAsync(), "Id", "Text", joke.JokeQuestionId);
                    ViewBag.JokeAnswerId = new SelectList(await _context.JokeAnswers.ToListAsync(), "Id", "Text", joke.JokeAnswerId);
                    return View(joke);
                }
                joke.LikesCount = 0;

                _context.Add(joke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.JokeQuestionId = new SelectList(await _context.JokeQuestions.ToListAsync(), "Id", "Text", joke.JokeQuestionId);
            ViewBag.JokeAnswerId = new SelectList(await _context.JokeAnswers.ToListAsync(), "Id", "Text", joke.JokeAnswerId);
            return View(joke);
        }

        // POST: Jokes/Like/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(int id)
        {
            var joke = await _context.Jokes.FindAsync(id);
            if (joke != null)
            {
                joke.LikesCount++;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Jokes/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddComment(int jokeId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                return RedirectToAction(nameof(Details), new { id = jokeId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var comment = new Comment
                {
                    Text = commentText.Trim(),
                    JokeId = jokeId,
                    UserId = userId,
                    CreatedAt = DateTime.Now
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = jokeId });
        }

        // GET: Jokes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Jokes.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }

            // Security check: User must be the owner
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (joke.UserId != currentUserId)
            {
                return Forbid();
            }

            ViewBag.JokeQuestionId = new SelectList(await _context.JokeQuestions.ToListAsync(), "Id", "Text", joke.JokeQuestionId);
            ViewBag.JokeAnswerId = new SelectList(await _context.JokeAnswers.ToListAsync(), "Id", "Text", joke.JokeAnswerId);
            return View(joke);
        }

        // POST: Jokes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JokeQuestionId,JokeAnswerId,UserId,LikesCount")] Joke joke)
        {
            if (id != joke.Id)
            {
                return NotFound();
            }

            // Security check: User must be the owner
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (joke.UserId != currentUserId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(joke.Id))
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

            ViewBag.JokeQuestionId = new SelectList(await _context.JokeQuestions.ToListAsync(), "Id", "Text", joke.JokeQuestionId);
            ViewBag.JokeAnswerId = new SelectList(await _context.JokeAnswers.ToListAsync(), "Id", "Text", joke.JokeAnswerId);
            return View(joke);
        }

        // GET: Jokes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Jokes
                .Include(j => j.JokeQuestion)
                .Include(j => j.JokeAnswer)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (joke == null)
            {
                return NotFound();
            }

            // Security check: User must be the owner
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (joke.UserId != currentUserId)
            {
                return Forbid();
            }

            return View(joke);
        }

        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var joke = await _context.Jokes.FindAsync(id);
            if (joke != null)
            {
                // Security check: User must be the owner
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (joke.UserId != currentUserId)
                {
                    return Forbid();
                }

                _context.Jokes.Remove(joke);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool JokeExists(int id)
        {
            return _context.Jokes.Any(e => e.Id == id);
        }
    }
}
