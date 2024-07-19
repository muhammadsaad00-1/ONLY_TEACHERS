using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using OT.Data;
using OT.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace OT.Pages.Contacts
{
    [Authorize]
    public class SubscriptionModel : PageModel
    {
        private readonly OTContext _context;
        private readonly UserManager<OTUser> _userManager;

        public SubscriptionModel(OTContext context, UserManager<OTUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET request handler
        public void OnGet()
        {
            // Nothing needs to be done here for a simple GET request
        }

        // POST request handler
        public async Task<IActionResult> OnPostAsync()
        {
            // Get the current user
            var currentUser = await _userManager.GetUserAsync(User);

            // Update the Paid attribute to 1
            currentUser.Paid = 1;

            // Save changes to the database
            await _userManager.UpdateAsync(currentUser);

            // Redirect to a success page or wherever needed
            return RedirectToPage("/Index");
        }
    }
}
