using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVCProject.Data;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly WebAppContext _context;

        public CommentController(UserManager<AppUser> userManager,WebAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(string? message,int? id)
        {
            if(!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "Please Login for writing comments";
                return RedirectToAction("login", "account");
            }
            if (message.IsNullOrEmpty()) 
            {
                TempData["EmptyComment"] = "Boshluq olmaz!";
                return RedirectToAction("details", "blog", new {id = id}); 
            }
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Comment comment = new Comment();
            comment.AppUserId = user.Id;
            comment.User= user;
            comment.Content = message;
            comment.Date = DateTime.Now;
            comment.BlogId = (int)id;

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
                      
            return RedirectToAction("details", "blog", new { id = id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "Please Login for writing comments";
                return RedirectToAction("login", "account");
            }
            
            if (id == null || id == 0) return NotFound();

            Comment? deletComment = await _context.Comments.Include(c=>c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (deletComment == null) return NotFound();

            if(deletComment.User.UserName != User.Identity.Name) return Content("Agilli ol !");
          
            _context.Comments.Remove(deletComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("details", "blog", new { id = deletComment.BlogId });

        }
    }
}
