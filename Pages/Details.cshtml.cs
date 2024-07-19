using OT.Authorization;
using OT.Data;
using OT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using OT.Areas.Identity.Data;
using ContactManager.Authorization;

namespace OT.Pages.Contacts
{
    #region snippet
    [Authorize]
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(
            OTContext context,
            IAuthorizationService AuthorizationService,
            UserManager<OTUser> userManager)
            : base(context, AuthorizationService, userManager)
        {
        }

        public Post Post { get; set; }
        

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await Context.Post.FirstOrDefaultAsync(m => m.Id == id);

            if (Post== null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole("Manager") ||
                               User.IsInRole("Admin");
           


            var currentUserId = UserManager.GetUserId(User);
               var userObj = await UserManager.FindByIdAsync(currentUserId);
            if(userObj.UserType == "Teacher" && currentUserId != Post.OwnerID){
                return Forbid();
            }
            if(userObj.UserType == "Student" && userObj.Paid == 0){
                return RedirectToPage("Subscription");
            }
          


            if (!isAuthorized
                && currentUserId != Post.OwnerID
                && Post.Status != ContactStatus.Approved)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, ContactStatus status)
        {
            var contact = await Context.Post.FirstOrDefaultAsync(
                                                      m => m.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            var contactOperation = (status == ContactStatus.Approved)
                                                       ? "Approve"
                                                       : "Reject";

var isAuthorized = await AuthorizationService.AuthorizeAsync(User, contact,
                            new[] { ContactOperations.Read });

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            contact.Status = status;
            Context.Post.Update(contact);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
    #endregion
}
