// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.EntityFrameworkCore;
// using OT.Data;
// using OT.Models;

// namespace MvcMovie.Pages;
// [Authorize]
// [AllowAnonymous] 
// public class IndexModel : PageModel
// {
 
//         private readonly OTContext _context;

//         public IndexModel(OTContext context)
//         {
//             _context = context;
//         }

//         public IList<Post> Posts { get; set; }

//         public async Task<IActionResult> OnGetAsync()
//         {
//             Posts = await _context.Post.ToListAsync();
//             return Page();
//         }
// }
