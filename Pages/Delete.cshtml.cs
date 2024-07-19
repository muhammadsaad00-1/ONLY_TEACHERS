using OT.Authorization;
using OT.Data;
using OT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using OT.Areas.Identity.Data;
using ContactManager.Authorization;

namespace OT.Pages.Contacts
{
    #region snippet
    [Authorize]
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(
            OTContext context,
            IAuthorizationService authorizationService,
            UserManager<OTUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Post Post { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await Context.Post.FirstOrDefaultAsync(
                                                 m => m.Id == id);

            if (Post == null)
            {
                return NotFound();
            }
            

var isAuthorized = await AuthorizationService.AuthorizeAsync(
                          User, Post,
                          new[] { ContactOperations.Delete });

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var contact = await Context
                .Post.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

           var isAuthorized = await AuthorizationService.AuthorizeAsync(
                          User, contact,
                          new[] { ContactOperations.Delete });

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Post.Remove(contact);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
    #endregion
}
