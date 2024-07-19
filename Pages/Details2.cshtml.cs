using OT.Authorization;
using OT.Data;
using OT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OT.Areas.Identity.Data;
using OT.Data;
using OT.Models;
using System.Threading.Tasks;

namespace OT.Pages.Contacts
{
    #region snippet
    [Authorize]
    [AllowAnonymous]
    public class Details2Model : DI_BasePageModel
    {
        public Details2Model(
            OTContext context,
            IAuthorizationService authorizationService,
            UserManager<OTUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public Post Post  { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await Context.Post.FirstOrDefaultAsync(m => m.Id == id);

            if (Post == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            var isAuthorized = User.IsInRole("Manager") ||
                               User.IsInRole("Admin");

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Post.OwnerID
                && Post.Status != ContactStatus.Approved)
            {
                return Forbid();
            }

            return Page();
        }
    }
    #endregion
}
